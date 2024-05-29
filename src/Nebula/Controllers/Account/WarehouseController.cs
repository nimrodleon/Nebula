using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Account;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/account/{companyId}/[controller]")]
[ApiController]
public class WarehouseController(IWarehouseService warehouseService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Name" };
        var warehouses = await warehouseService.GetFilteredAsync(companyId, fieldNames, query);
        return Ok(warehouses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var warehouse = await warehouseService.GetByIdAsync(companyId, id);
        return Ok(warehouse);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Warehouse model)
    {
        model.CompanyId = companyId.Trim();
        model.Name = model.Name.ToUpper();
        await warehouseService.InsertOneAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Warehouse model)
    {
        var warehouse = await warehouseService.GetByIdAsync(companyId, id);

        model.Id = warehouse.Id;
        model.CompanyId = companyId.Trim();
        model.Name = model.Name.ToUpper();
        model = await warehouseService.ReplaceOneAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var warehouse = await warehouseService.GetByIdAsync(companyId, id);
        await warehouseService.DeleteOneAsync(companyId, id);
        return Ok(warehouse);
    }
}
