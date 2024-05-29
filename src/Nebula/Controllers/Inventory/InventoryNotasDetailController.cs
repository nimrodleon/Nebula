using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Notas;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class InventoryNotasDetailController(IInventoryNotasDetailService inventoryNotasDetailService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var responseData = await inventoryNotasDetailService.GetListAsync(companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var inventoryNotasDetail = await inventoryNotasDetailService.GetByIdAsync(companyId, id);
        return Ok(inventoryNotasDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] InventoryNotasDetail model)
    {
        model.CompanyId = companyId.Trim();
        var inventoryNotasDetail = await inventoryNotasDetailService.InsertOneAsync(model);
        return Ok(inventoryNotasDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] InventoryNotasDetail model)
    {
        var inventoryNotasDetail = await inventoryNotasDetailService.GetByIdAsync(companyId, id);
        model.Id = inventoryNotasDetail.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await inventoryNotasDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var inventoryNotasDetail = await inventoryNotasDetailService.GetByIdAsync(companyId, id);
        await inventoryNotasDetailService.DeleteOneAsync(companyId, inventoryNotasDetail.Id);
        return Ok(inventoryNotasDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await inventoryNotasDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }
}
