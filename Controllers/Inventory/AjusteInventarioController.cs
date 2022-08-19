using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Services.Inventory;
using Nebula.Database.ViewModels.Common;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class AjusteInventarioController : ControllerBase
{
    private readonly AjusteInventarioService _ajusteInventarioService;
    private readonly AjusteInventarioDetailService _ajusteInventarioDetailService;

    public AjusteInventarioController(AjusteInventarioService ajusteInventarioService, AjusteInventarioDetailService ajusteInventarioDetailService)
    {
        _ajusteInventarioService = ajusteInventarioService;
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
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
        var ajusteInventario = await _ajusteInventarioService.GetAsync(id);
        return Ok(ajusteInventario);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] AjusteInventario model)
    {
        var ajusteInventario = await _ajusteInventarioService.CreateAsync(model);
        await _ajusteInventarioDetailService.GenerateDetailAsync(ajusteInventario.LocationId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AjusteInventario model)
    {
        var ajusteInventario = await _ajusteInventarioService.GetAsync(id);
        model.Id = ajusteInventario.Id;
        var responseData = await _ajusteInventarioService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetAsync(id);
        await _ajusteInventarioService.RemoveAsync(ajusteInventario.Id);
        return Ok(ajusteInventario);
    }
}
