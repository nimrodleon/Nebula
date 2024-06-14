using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Finanzas.Dto;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Finanzas.Schemas;
using Nebula.Common.Helpers;

namespace Nebula.Controllers.Finanzas;

[Authorize]
[PersonalAuthorize(UserRole = UserRole.User)]
[Route("api/finanzas/[controller]")]
[ApiController]
public class ReceivableController(
    IUserAuthenticationService userAuthenticationService,
    IAccountsReceivableService receivableService,
    ICashierDetailService cashierDetailService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] CuentaPorCobrarMensualParamSchema requestParam,
        [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var cuentasPorCobrar = await receivableService.GetListAsync(_companyId, requestParam, page, pageSize);
        var totalCuentasPorCobrar = await receivableService.GetTotalCargosAsync(_companyId, requestParam);
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
    public async Task<IActionResult> Show(string id)
    {
        var receivable = await receivableService.GetByIdAsync(_companyId, id);
        return Ok(receivable);
    }

    [HttpGet("GetReceivablesByContactId/{contactId}")]
    public async Task<IActionResult> GetReceivablesByContactId(string contactId,
        [FromQuery] CuentaPorCobrarMensualParamSchema requestParam)
    {
        var receivable = await receivableService.GetReceivablesByContactId(_companyId, contactId, requestParam);
        return Ok(receivable);
    }

    [HttpGet("GetCargosCliente")]
    public async Task<IActionResult> GetCargosCliente([FromQuery] CuentaPorCobrarClienteAnualParamSchema @params)
    {
        var receivable = await receivableService.GetCargosClienteAnual(_companyId, @params);
        return Ok(receivable);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AccountsReceivable model)
    {
        model.CompanyId = _companyId.Trim();
        await receivableService.CreateAsync(cashierDetailService, model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AccountsReceivable model)
    {
        var receivable = await receivableService.GetByIdAsync(_companyId, id);
        model.Id = receivable.Id;
        model.CompanyId = _companyId.Trim();
        receivable = await receivableService.ReplaceOneAsync(receivable.Id, model);
        return Ok(receivable);
    }

    [HttpDelete("{id}"), PersonalAuthorize(UserRole = UserRole.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var receivable = await receivableService.GetByIdAsync(_companyId, id);
        if (receivable.Type == "CARGO")
            await receivableService.DeleteOneAsync(_companyId, receivable.Id);
        if (receivable.Type == "ABONO")
            await receivableService.RemoveAbonoAsync(_companyId, receivable);
        return Ok(receivable);
    }

    [HttpGet("Abonos/{id}")]
    public async Task<IActionResult> Abonos(string id)
    {
        var receivables = await receivableService.GetAbonosAsync(_companyId, id);
        return Ok(receivables);
    }

    [HttpGet("TotalAbonos/{id}")]
    public async Task<IActionResult> TotalAbonos(string id)
    {
        var total = await receivableService.GetTotalAbonosAsync(_companyId, id);
        return Ok(total);
    }

    [AllowAnonymous]
    [HttpGet("ExportDeudaExcel/{contactId}")]
    public async Task<IActionResult> ExportDeudaExcel(string contactId, [FromQuery] string year)
    {
        List<AccountsReceivable> cuentasPorCobrar =
            await receivableService.GetReceivablesByContactId(_companyId, contactId, year);
        ExportarCuentasPorCobrarDto exportar = new ExportarCuentasPorCobrarDto(cuentasPorCobrar);
        string pathExcel = exportar.GenerarArchivoExcel();
        FileStream stream = new FileStream(pathExcel, FileMode.Open);
        return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("PendientesPorCobrar")]
    public async Task<IActionResult> PendientesPorCobrar()
    {
        var cuentasPorCobrar = await receivableService.GetPendientesPorCobrar(_companyId);

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
