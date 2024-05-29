using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Account;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/account/[controller]")]
[ApiController]
public class WarehouseController(
    IUserAuthenticationService userAuthenticationService,
    IWarehouseService warehouseService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Name" };
        var warehouses = await warehouseService.GetFilteredAsync(_companyId, fieldNames, query);
        return Ok(warehouses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var warehouse = await warehouseService.GetByIdAsync(_companyId, id);
        return Ok(warehouse);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Warehouse model)
    {
        model.CompanyId = _companyId.Trim();
        model.Name = model.Name.ToUpper();
        await warehouseService.InsertOneAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Warehouse model)
    {
        var warehouse = await warehouseService.GetByIdAsync(_companyId, id);

        model.Id = warehouse.Id;
        model.CompanyId = _companyId.Trim();
        model.Name = model.Name.ToUpper();
        model = await warehouseService.ReplaceOneAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var warehouse = await warehouseService.GetByIdAsync(_companyId, id);
        await warehouseService.DeleteOneAsync(_companyId, id);
        return Ok(warehouse);
    }
}
