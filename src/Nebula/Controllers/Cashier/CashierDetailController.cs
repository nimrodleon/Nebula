using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Cashier.Models;

namespace Nebula.Controllers.Cashier;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/cashier/{companyId}/[controller]")]
[ApiController]
public class CashierDetailController : ControllerBase
{
    private readonly ICashierDetailService _cashierDetailService;

    public CashierDetailController(ICashierDetailService cashierDetailService)
    {
        _cashierDetailService = cashierDetailService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id, [FromQuery] string query = "")
    {
        if (string.IsNullOrEmpty(query)) query = string.Empty;
        var responseData = await _cashierDetailService.GetListAsync(companyId, id, query);
        return Ok(responseData);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] CashierDetail model)
    {
        if (model.TypeOperation == TipoOperationCaja.EntradaDeDinero)
            model.TypeOperation = TipoOperationCaja.EntradaDeDinero;
        if (model.TypeOperation == TipoOperationCaja.SalidaDeDinero)
            model.TypeOperation = TipoOperationCaja.SalidaDeDinero;
        model.FormaPago = MetodosPago.Contado;
        model.CompanyId = companyId.Trim();
        model = await _cashierDetailService.CreateAsync(model);
        return Ok(model);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await _cashierDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }

    [HttpGet("ResumenCaja/{id}")]
    public async Task<IActionResult> ResumenCaja(string companyId, string id)
    {
        var resumenCaja = await _cashierDetailService.GetResumenCaja(companyId, id);
        return Ok(resumenCaja);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = CompanyRoles.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var cashierDetail = await _cashierDetailService.GetByIdAsync(companyId, id);
        await _cashierDetailService.RemoveAsync(companyId, cashierDetail.Id);
        return Ok(cashierDetail);
    }
}
