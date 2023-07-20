using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Notas;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Dto;
using Nebula.Modules.Configurations.Subscriptions;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class InventoryNotasController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IInventoryNotasService _inventoryNotasService;
    private readonly IValidateStockService _validateStockService;

    public InventoryNotasController(ISubscriptionService subscriptionService,
        IInventoryNotasService inventoryNotasService,
        IValidateStockService validateStockService)
    {
        _subscriptionService = subscriptionService;
        _inventoryNotasService = inventoryNotasService;
        _validateStockService = validateStockService;
    }

    [HttpGet("Index"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await _inventoryNotasService.GetListAsync(model);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Show(string id)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(id);
        return Ok(inventoryNotas);
    }

    [HttpPost("Create"), UserAuthorize(Permission.InventoryCreate)]
    public async Task<IActionResult> Create([FromBody] InventoryNotas model)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var inventoryNotas = await _inventoryNotasService.CreateAsync(model);
        return Ok(inventoryNotas);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] InventoryNotas model)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(id);
        model.Id = inventoryNotas.Id;
        var responseData = await _inventoryNotasService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.InventoryDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(id);
        await _inventoryNotasService.RemoveAsync(inventoryNotas.Id);
        return Ok(inventoryNotas);
    }

    [HttpGet("Validate/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Validate(string id)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var inventoryNotas = await _validateStockService.ValidarNotas(id);
        return Ok(inventoryNotas);
    }
}
