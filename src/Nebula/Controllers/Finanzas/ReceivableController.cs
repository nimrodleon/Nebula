using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Finanzas.Dto;
using Nebula.Modules.Finanzas.Models;

namespace Nebula.Controllers.Finanzas;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/finanzas/{companyId}/[controller]")]
[ApiController]
public class ReceivableController : ControllerBase
{
    private readonly IReceivableService _receivableService;
    private readonly ICashierDetailService _cashierDetailService;

    public ReceivableController(
        IReceivableService receivableService,
        ICashierDetailService cashierDetailService)
    {
        _receivableService = receivableService;
        _cashierDetailService = cashierDetailService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] ReceivableRequestParams requestParam)
    {
        var responseData = await _receivableService.GetListAsync(companyId, requestParam);
        return Ok(responseData);
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
        [FromQuery] ReceivableRequestParams requestParam)
    {
        var receivable = await _receivableService.GetReceivablesByContactId(companyId, contactId, requestParam);
        return Ok(receivable);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Receivable model)
    {
        model.CompanyId = companyId.Trim();
        await _receivableService.CreateAsync(_cashierDetailService, model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Receivable model)
    {
        var receivable = await _receivableService.GetByIdAsync(companyId, id);
        model.Id = receivable.Id;
        model.CompanyId = companyId.Trim();
        receivable = await _receivableService.UpdateAsync(receivable.Id, model);
        return Ok(receivable);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = CompanyRoles.Admin)]
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
        List<Receivable> cuentasPorCobrar = await _receivableService.GetReceivablesByContactId(companyId, contactId, year);
        ExportarCuentasPorCobrarDto exportar = new ExportarCuentasPorCobrarDto(cuentasPorCobrar);
        string pathExcel = exportar.GenerarArchivoExcel();
        FileStream stream = new FileStream(pathExcel, FileMode.Open);
        return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
