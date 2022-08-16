using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Services.Inventory;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class InventoryNotasDetailController : ControllerBase
{
    private readonly InventoryNotasDetailService _inventoryNotasDetailService;

    public InventoryNotasDetailController(InventoryNotasDetailService inventoryNotasDetailService)
    {
        _inventoryNotasDetailService = inventoryNotasDetailService;
    }

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await _inventoryNotasDetailService.GetListAsync(id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetAsync(id);
        return Ok(inventoryNotasDetail);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] InventoryNotasDetail model)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.CreateAsync(model);
        return Ok(inventoryNotasDetail);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] InventoryNotasDetail model)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetAsync(id);
        model.Id = inventoryNotasDetail.Id;
        var responseData = await _inventoryNotasDetailService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetAsync(id);
        await _inventoryNotasDetailService.RemoveAsync(inventoryNotasDetail.Id);
        return Ok(inventoryNotasDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await _inventoryNotasDetailService.CountDocumentsAsync(id);
        return Ok(countDocuments);
    }
}
