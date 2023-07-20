using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Taller.Models;
using Nebula.Modules.Taller.Services;

namespace Nebula.Controllers.Taller;

[Route("api/taller/[controller]")]
[ApiController]
public class ItemRepairOrderController : ControllerBase
{
    private readonly ITallerItemRepairOrderService _itemRepairOrderService;

    public ItemRepairOrderController(ITallerItemRepairOrderService itemRepairOrderService)
    {
        _itemRepairOrderService = itemRepairOrderService;
    }

    [HttpGet("Index/{id}"), UserAuthorize(Permission.MaterialTallerRead)]
    public async Task<IActionResult> Index(string id)
    {
        var itemsRepairOrder = await _itemRepairOrderService.GetItemsRepairOrder(id);
        return Ok(itemsRepairOrder);
    }

    [HttpPost("Create"), UserAuthorize(Permission.MaterialTallerCreate)]
    public async Task<IActionResult> Create([FromBody] TallerItemRepairOrder model)
    {
        await _itemRepairOrderService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.MaterialTallerEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] TallerItemRepairOrder model)
    {
        var itemRepairOrder = await _itemRepairOrderService.GetByIdAsync(id);
        model.Id = itemRepairOrder.Id;
        itemRepairOrder = await _itemRepairOrderService.UpdateAsync(itemRepairOrder.Id, model);
        return Ok(itemRepairOrder);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.MaterialTallerDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var itemRepairOrder = await _itemRepairOrderService.GetByIdAsync(id);
        await _itemRepairOrderService.RemoveAsync(itemRepairOrder.Id);
        return Ok(itemRepairOrder);
    }
}
