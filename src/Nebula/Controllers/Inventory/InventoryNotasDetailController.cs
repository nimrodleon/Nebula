using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Notas;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class InventoryNotasDetailController(
    IUserAuthenticationService userAuthenticationService,
    IInventoryNotasDetailService inventoryNotasDetailService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await inventoryNotasDetailService.GetListAsync(_companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var inventoryNotasDetail = await inventoryNotasDetailService.GetByIdAsync(_companyId, id);
        return Ok(inventoryNotasDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryNotasDetail model)
    {
        model.CompanyId = _companyId.Trim();
        var inventoryNotasDetail = await inventoryNotasDetailService.InsertOneAsync(model);
        return Ok(inventoryNotasDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] InventoryNotasDetail model)
    {
        var inventoryNotasDetail = await inventoryNotasDetailService.GetByIdAsync(_companyId, id);
        model.Id = inventoryNotasDetail.Id;
        model.CompanyId = _companyId.Trim();
        var responseData = await inventoryNotasDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var inventoryNotasDetail = await inventoryNotasDetailService.GetByIdAsync(_companyId, id);
        await inventoryNotasDetailService.DeleteOneAsync(_companyId, inventoryNotasDetail.Id);
        return Ok(inventoryNotasDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await inventoryNotasDetailService.CountDocumentsAsync(_companyId, id);
        return Ok(countDocuments);
    }
}
