using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Notas;

namespace Nebula.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class InventoryNotasDetailController : ControllerBase
{
    private readonly IInventoryNotasDetailService _inventoryNotasDetailService;

    public InventoryNotasDetailController(IInventoryNotasDetailService inventoryNotasDetailService)
    {
        _inventoryNotasDetailService = inventoryNotasDetailService;
    }

    [HttpGet("Index/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await _inventoryNotasDetailService.GetListAsync(id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Show(string id)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetByIdAsync(id);
        return Ok(inventoryNotasDetail);
    }

    [HttpPost("Create"), UserAuthorize(Permission.InventoryCreate)]
    public async Task<IActionResult> Create([FromBody] InventoryNotasDetail model)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.CreateAsync(model);
        return Ok(inventoryNotasDetail);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] InventoryNotasDetail model)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetByIdAsync(id);
        model.Id = inventoryNotasDetail.Id;
        var responseData = await _inventoryNotasDetailService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.InventoryDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetByIdAsync(id);
        await _inventoryNotasDetailService.RemoveAsync(inventoryNotasDetail.Id);
        return Ok(inventoryNotasDetail);
    }

    [HttpGet("CountDocuments/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await _inventoryNotasDetailService.CountDocumentsAsync(id);
        return Ok(countDocuments);
    }
}
