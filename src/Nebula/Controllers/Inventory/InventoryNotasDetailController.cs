using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Notas;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class InventoryNotasDetailController : ControllerBase
{
    private readonly IInventoryNotasDetailService _inventoryNotasDetailService;

    public InventoryNotasDetailController(IInventoryNotasDetailService inventoryNotasDetailService)
    {
        _inventoryNotasDetailService = inventoryNotasDetailService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var responseData = await _inventoryNotasDetailService.GetListAsync(companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetByIdAsync(companyId, id);
        return Ok(inventoryNotasDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] InventoryNotasDetail model)
    {
        model.CompanyId = companyId.Trim();
        var inventoryNotasDetail = await _inventoryNotasDetailService.CreateAsync(model);
        return Ok(inventoryNotasDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] InventoryNotasDetail model)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetByIdAsync(companyId, id);
        model.Id = inventoryNotasDetail.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await _inventoryNotasDetailService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var inventoryNotasDetail = await _inventoryNotasDetailService.GetByIdAsync(companyId, id);
        await _inventoryNotasDetailService.RemoveAsync(companyId, inventoryNotasDetail.Id);
        return Ok(inventoryNotasDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await _inventoryNotasDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }
}
