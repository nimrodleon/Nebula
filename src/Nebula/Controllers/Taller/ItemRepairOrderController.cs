using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Plugins.Taller.Models;
using Nebula.Plugins.Taller.Services;

namespace Nebula.Controllers.Taller;

[Authorize(Roles = AuthRoles.User)]
[Route("api/taller/[controller]")]
[ApiController]
public class ItemRepairOrderController : ControllerBase
{
    private readonly TallerItemRepairOrderService _itemRepairOrderService;

    public ItemRepairOrderController(TallerItemRepairOrderService itemRepairOrderService)
    {
        _itemRepairOrderService = itemRepairOrderService;
    }

    [HttpGet("Index/{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var itemsRepairOrder = await _itemRepairOrderService.GetItemsRepairOrder(id);
        return Ok(itemsRepairOrder);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] TallerItemRepairOrder model)
    {
        await _itemRepairOrderService.CreateAsync(model);
        return Ok(model);
    }
}
