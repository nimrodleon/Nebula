using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Dto;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class AjusteInventarioController : ControllerBase
{
    private readonly IAjusteInventarioService _ajusteInventarioService;
    private readonly IAjusteInventarioDetailService _ajusteInventarioDetailService;
    private readonly IValidateStockService _validateStockService;

    public AjusteInventarioController(
        IAjusteInventarioService ajusteInventarioService,
        IAjusteInventarioDetailService ajusteInventarioDetailService,
        IValidateStockService validateStockService)
    {
        _ajusteInventarioService = ajusteInventarioService;
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
        _validateStockService = validateStockService;
    }

    [HttpGet("Index"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await _ajusteInventarioService.GetListAsync(model);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Show(string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(id);
        return Ok(ajusteInventario);
    }

    [HttpPost("Create"), UserAuthorize(Permission.InventoryCreate)]
    public async Task<IActionResult> Create([FromBody] AjusteInventario model)
    {
        var ajusteInventario = await _ajusteInventarioService.CreateAsync(model);
        await _ajusteInventarioDetailService.GenerateDetailAsync(ajusteInventario.LocationId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] AjusteInventario model)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(id);
        model.Id = ajusteInventario.Id;
        var responseData = await _ajusteInventarioService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.InventoryDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(id);
        await _ajusteInventarioService.RemoveAsync(ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpGet("Validate/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Validate(string id)
    {
        var ajusteInventario = await _validateStockService.ValidarAjusteInventario(id);
        return Ok(ajusteInventario);
    }
}
