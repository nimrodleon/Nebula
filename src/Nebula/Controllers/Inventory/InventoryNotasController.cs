using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Services.Inventory;
using Nebula.Database.Dto.Common;
using Nebula.Database.Services.Common;
using Nebula.Modules.Inventory.Stock;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class InventoryNotasController : ControllerBase
{
    private readonly InventoryNotasService _inventoryNotasService;
    private readonly ValidateStockService _validateStockService;
    private readonly ConfigurationService _configurationService;

    public InventoryNotasController(InventoryNotasService inventoryNotasService,
        ValidateStockService validateStockService, ConfigurationService configurationService)
    {
        _inventoryNotasService = inventoryNotasService;
        _validateStockService = validateStockService;
        _configurationService = configurationService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await _inventoryNotasService.GetListAsync(model);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(id);
        return Ok(inventoryNotas);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] InventoryNotas model)
    {
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var inventoryNotas = await _inventoryNotasService.CreateAsync(model);
        return Ok(inventoryNotas);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] InventoryNotas model)
    {
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(id);
        model.Id = inventoryNotas.Id;
        var responseData = await _inventoryNotasService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(id);
        await _inventoryNotasService.RemoveAsync(inventoryNotas.Id);
        return Ok(inventoryNotas);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string id)
    {
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var inventoryNotas = await _validateStockService.ValidarNotas(id);
        return Ok(inventoryNotas);
    }
}
