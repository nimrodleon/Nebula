using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Taller.Models;
using Nebula.Modules.Taller.Services;

namespace Nebula.Controllers.Taller;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/taller/[controller]")]
[ApiController]
public class ItemRepairOrderController(
    IUserAuthenticationService userAuthenticationService,
    ITallerItemRepairOrderService itemRepairOrderService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var itemsRepairOrder = await itemRepairOrderService.GetItemsRepairOrder(_companyId, id);
        return Ok(itemsRepairOrder);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TallerItemRepairOrder model)
    {
        model.CompanyId = _companyId.Trim();
        model = await itemRepairOrderService.InsertOneAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] TallerItemRepairOrder model)
    {
        var itemRepairOrder = await itemRepairOrderService.GetByIdAsync(_companyId, id);
        model.Id = itemRepairOrder.Id;
        model.CompanyId = _companyId.Trim();
        itemRepairOrder = await itemRepairOrderService.ReplaceOneAsync(itemRepairOrder.Id, model);
        return Ok(itemRepairOrder);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var itemRepairOrder = await itemRepairOrderService.GetByIdAsync(_companyId, id);
        await itemRepairOrderService.DeleteOneAsync(_companyId, itemRepairOrder.Id);
        return Ok(itemRepairOrder);
    }
}
