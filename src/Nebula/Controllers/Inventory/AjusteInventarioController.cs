using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Dto;
using Nebula.Modules.Configurations.Subscriptions;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class AjusteInventarioController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly AjusteInventarioService _ajusteInventarioService;
    private readonly AjusteInventarioDetailService _ajusteInventarioDetailService;
    private readonly ValidateStockService _validateStockService;

    public AjusteInventarioController(ISubscriptionService subscriptionService,
        AjusteInventarioService ajusteInventarioService,
        AjusteInventarioDetailService ajusteInventarioDetailService, ValidateStockService validateStockService)
    {
        _subscriptionService = subscriptionService;
        _ajusteInventarioService = ajusteInventarioService;
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
        _validateStockService = validateStockService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await _ajusteInventarioService.GetListAsync(model);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(id);
        return Ok(ajusteInventario);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] AjusteInventario model)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var ajusteInventario = await _ajusteInventarioService.CreateAsync(model);
        await _ajusteInventarioDetailService.GenerateDetailAsync(ajusteInventario.LocationId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AjusteInventario model)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(id);
        model.Id = ajusteInventario.Id;
        var responseData = await _ajusteInventarioService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(id);
        await _ajusteInventarioService.RemoveAsync(ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string id)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var ajusteInventario = await _validateStockService.ValidarAjusteInventario(id);
        return Ok(ajusteInventario);
    }
}
