using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Cashier;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Configurations;

namespace Nebula.Controllers.Cashier;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class CashierDetailController : ControllerBase
{
    private readonly ConfigurationService _configurationService;
    private readonly CashierDetailService _cashierDetailService;

    public CashierDetailController(ConfigurationService configurationService, CashierDetailService cashierDetailService)
    {
        _configurationService = configurationService;
        _cashierDetailService = cashierDetailService;
    }

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(string id, [FromQuery] string? query)
    {
        if (string.IsNullOrEmpty(query)) query = string.Empty;
        var responseData = await _cashierDetailService.GetListAsync(id, query);
        return Ok(responseData);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CashierDetail model)
    {
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        if (model.TypeOperation == TypeOperationCaja.EntradaDeDinero)
            model.TypeOperation = TypeOperationCaja.EntradaDeDinero;
        if (model.TypeOperation == TypeOperationCaja.SalidaDeDinero)
            model.TypeOperation = TypeOperationCaja.SalidaDeDinero;
        model.FormaPago = FormaPago.Contado;
        await _cashierDetailService.CreateAsync(model);
        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = "La operación ha sido registrado!"
        });
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await _cashierDetailService.CountDocumentsAsync(id);
        return Ok(countDocuments);
    }

    [HttpGet("ResumenCaja/{id}")]
    public async Task<IActionResult> ResumenCaja(string id)
    {
        var resumenCaja = await _cashierDetailService.GetResumenCaja(id);
        return Ok(resumenCaja);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var cashierDetail = await _cashierDetailService.GetByIdAsync(id);
        await _cashierDetailService.RemoveAsync(cashierDetail.Id);
        return Ok(new { Ok = true, Data = cashierDetail, Msg = "El detalle de caja ha sido borrado!" });
    }
}
