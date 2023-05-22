using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Dto.Common;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Configurations;
using Nebula.Modules.Auth.Helpers;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class AjusteInventarioController : ControllerBase
{
    private readonly ConfigurationService _configurationService;
    private readonly AjusteInventarioService _ajusteInventarioService;
    private readonly AjusteInventarioDetailService _ajusteInventarioDetailService;
    private readonly ValidateStockService _validateStockService;

    public AjusteInventarioController(AjusteInventarioService ajusteInventarioService, ConfigurationService configurationService,
        AjusteInventarioDetailService ajusteInventarioDetailService, ValidateStockService validateStockService)
    {
        _ajusteInventarioService = ajusteInventarioService;
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
        _validateStockService = validateStockService;
        _configurationService = configurationService;
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
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var ajusteInventario = await _ajusteInventarioService.CreateAsync(model);
        await _ajusteInventarioDetailService.GenerateDetailAsync(ajusteInventario.LocationId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AjusteInventario model)
    {
        var license = await _configurationService.ValidarAcceso();
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
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var ajusteInventario = await _validateStockService.ValidarAjusteInventario(id);
        return Ok(ajusteInventario);
    }
}
