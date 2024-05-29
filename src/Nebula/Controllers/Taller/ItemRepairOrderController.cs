using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Taller.Models;
using Nebula.Modules.Taller.Services;

namespace Nebula.Controllers.Taller;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/taller/{companyId}/[controller]")]
[ApiController]
public class ItemRepairOrderController(ITallerItemRepairOrderService itemRepairOrderService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var itemsRepairOrder = await itemRepairOrderService.GetItemsRepairOrder(companyId, id);
        return Ok(itemsRepairOrder);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] TallerItemRepairOrder model)
    {
        model.CompanyId = companyId.Trim();
        model = await itemRepairOrderService.InsertOneAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] TallerItemRepairOrder model)
    {
        var itemRepairOrder = await itemRepairOrderService.GetByIdAsync(companyId, id);
        model.Id = itemRepairOrder.Id;
        model.CompanyId = companyId.Trim();
        itemRepairOrder = await itemRepairOrderService.ReplaceOneAsync(itemRepairOrder.Id, model);
        return Ok(itemRepairOrder);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var itemRepairOrder = await itemRepairOrderService.GetByIdAsync(companyId, id);
        await itemRepairOrderService.DeleteOneAsync(companyId, itemRepairOrder.Id);
        return Ok(itemRepairOrder);
    }
}
