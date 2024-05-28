using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Finanzas.Dto;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Finanzas.Schemas;
using Nebula.Common.Helpers;

namespace Nebula.Controllers.Finanzas;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/finanzas/{companyId}/[controller]")]
[ApiController]
public class ReceivableController : ControllerBase
{
    private readonly IAccountsReceivableService _receivableService;
    private readonly ICashierDetailService _cashierDetailService;

    public ReceivableController(
        IAccountsReceivableService receivableService,
        ICashierDetailService cashierDetailService)
    {
        _receivableService = receivableService;
        _cashierDetailService = cashierDetailService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] CuentaPorCobrarMensualParamSchema requestParam, [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var cuentasPorCobrar = await _receivableService.GetListAsync(companyId, requestParam, page, pageSize);
        var totalCuentasPorCobrar = await _receivableService.GetTotalCargosAsync(companyId, requestParam);
        var totalPages = (int)Math.Ceiling((double)totalCuentasPorCobrar / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<AccountsReceivable>
        {
            Pagination = paginationInfo,
            Data = cuentasPorCobrar
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var receivable = await _receivableService.GetByIdAsync(companyId, id);
        return Ok(receivable);
    }

    [HttpGet("GetReceivablesByContactId/{contactId}")]
    public async Task<IActionResult> GetReceivablesByContactId(
        string companyId, string contactId,
        [FromQuery] CuentaPorCobrarMensualParamSchema requestParam)
    {
        var receivable = await _receivableService.GetReceivablesByContactId(companyId, contactId, requestParam);
        return Ok(receivable);
    }

    [HttpGet("GetCargosCliente")]
    public async Task<IActionResult> GetCargosCliente(string companyId,
        [FromQuery] CuentaPorCobrarClienteAnualParamSchema @params)
    {
        var receivable = await _receivableService.GetCargosClienteAnual(companyId, @params);
        return Ok(receivable);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] AccountsReceivable model)
    {
        model.CompanyId = companyId.Trim();
        await _receivableService.CreateAsync(_cashierDetailService, model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] AccountsReceivable model)
    {
        var receivable = await _receivableService.GetByIdAsync(companyId, id);
        model.Id = receivable.Id;
        model.CompanyId = companyId.Trim();
        receivable = await _receivableService.UpdateAsync(receivable.Id, model);
        return Ok(receivable);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var receivable = await _receivableService.GetByIdAsync(companyId, id);
        if (receivable.Type == "CARGO")
            await _receivableService.RemoveAsync(companyId, receivable.Id);
        if (receivable.Type == "ABONO")
            await _receivableService.RemoveAbonoAsync(companyId, receivable);
        return Ok(receivable);
    }

    [HttpGet("Abonos/{id}")]
    public async Task<IActionResult> Abonos(string companyId, string id)
    {
        var receivables = await _receivableService.GetAbonosAsync(companyId, id);
        return Ok(receivables);
    }

    [HttpGet("TotalAbonos/{id}")]
    public async Task<IActionResult> TotalAbonos(string companyId, string id)
    {
        var total = await _receivableService.GetTotalAbonosAsync(companyId, id);
        return Ok(total);
    }

    [AllowAnonymous]
    [HttpGet("ExportDeudaExcel/{contactId}")]
    public async Task<IActionResult> ExportDeudaExcel(string companyId, string contactId, [FromQuery] string year)
    {
        List<AccountsReceivable> cuentasPorCobrar = await _receivableService.GetReceivablesByContactId(companyId, contactId, year);
        ExportarCuentasPorCobrarDto exportar = new ExportarCuentasPorCobrarDto(cuentasPorCobrar);
        string pathExcel = exportar.GenerarArchivoExcel();
        FileStream stream = new FileStream(pathExcel, FileMode.Open);
        return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("PendientesPorCobrar")]
    public async Task<IActionResult> PendientesPorCobrar(string companyId)
    {
        var cuentasPorCobrar = await _receivableService.GetPendientesPorCobrar(companyId);

        // calcular el total a cobrar.
        decimal totalPorCobrar = cuentasPorCobrar.Sum(x => x.Saldo);

        // agrupar por cliente y calcular la deuda total por cliente.
        var deudasPorCliente = cuentasPorCobrar.GroupBy(x => x.ContactId)
            .Select(group => new DeudaClienteSchema
            {
                ContactId = group.Key,
                ContactName = group.First().ContactName,
                DeudaTotal = group.Sum(x => x.Saldo),
                Receivables = group.ToList()
            }).ToList();

        var resumen = new ResumenDeudaSchema
        {
            TotalPorCobrar = totalPorCobrar,
            DeudasPorCliente = deudasPorCliente
        };

        return Ok(resumen);
    }
}
