using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Taller.Models;
using Nebula.Modules.Taller.Services;

namespace Nebula.Controllers.Taller;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/taller/{companyId}/[controller]")]
[ApiController]
public class ItemRepairOrderController : ControllerBase
{
    private readonly ITallerItemRepairOrderService _itemRepairOrderService;

    public ItemRepairOrderController(ITallerItemRepairOrderService itemRepairOrderService)
    {
        _itemRepairOrderService = itemRepairOrderService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var itemsRepairOrder = await _itemRepairOrderService.GetItemsRepairOrder(companyId, id);
        return Ok(itemsRepairOrder);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] TallerItemRepairOrder model)
    {
        model.CompanyId = companyId.Trim();
        model = await _itemRepairOrderService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] TallerItemRepairOrder model)
    {
        var itemRepairOrder = await _itemRepairOrderService.GetByIdAsync(companyId, id);
        model.Id = itemRepairOrder.Id;
        model.CompanyId = companyId.Trim();
        itemRepairOrder = await _itemRepairOrderService.UpdateAsync(itemRepairOrder.Id, model);
        return Ok(itemRepairOrder);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var itemRepairOrder = await _itemRepairOrderService.GetByIdAsync(companyId, id);
        await _itemRepairOrderService.RemoveAsync(companyId, itemRepairOrder.Id);
        return Ok(itemRepairOrder);
    }
}
