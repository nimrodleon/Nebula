using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class AjusteInventarioDetailController : ControllerBase
{
    private readonly IAjusteInventarioDetailService _ajusteInventarioDetailService;

    public AjusteInventarioDetailController(IAjusteInventarioDetailService ajusteInventarioDetailService)
    {
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
    }

    [HttpGet("Index/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await _ajusteInventarioDetailService.GetListAsync(id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Show(string id)
    {
        var ajusteInventarioDetail = await _ajusteInventarioDetailService.GetByIdAsync(id);
        return Ok(ajusteInventarioDetail);
    }

    [HttpPost("Create"), UserAuthorize(Permission.InventoryCreate)]
    public async Task<IActionResult> Create([FromBody] AjusteInventarioDetail model)
    {
        var ajusteInventarioDetail = await _ajusteInventarioDetailService.CreateAsync(model);
        return Ok(ajusteInventarioDetail);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] AjusteInventarioDetail model)
    {
        var ajusteInventarioDetail = await _ajusteInventarioDetailService.GetByIdAsync(id);
        model.Id = ajusteInventarioDetail.Id;
        var responseData = await _ajusteInventarioDetailService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.InventoryDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var ajusteInventarioDetail = await _ajusteInventarioDetailService.GetByIdAsync(id);
        await _ajusteInventarioDetailService.RemoveAsync(ajusteInventarioDetail.Id);
        return Ok(ajusteInventarioDetail);
    }

    [HttpGet("CountDocuments/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await _ajusteInventarioDetailService.CountDocumentsAsync(id);
        return Ok(countDocuments);
    }
}
