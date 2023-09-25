using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;

namespace Nebula.Controllers.Account;

[Authorize]
[Route("api/{companyId}/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService) =>
        _warehouseService = warehouseService;

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Name" };
        var warehouses = await _warehouseService.GetFilteredAsync(companyId, fieldNames, query);
        return Ok(warehouses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(companyId, id);
        return Ok(warehouse);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Warehouse model)
    {
        model.CompanyId = companyId.Trim();
        model.Name = model.Name.ToUpper();
        await _warehouseService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Warehouse model)
    {
        var warehouse = await _warehouseService.GetByIdAsync(companyId, id);

        model.Id = warehouse.Id;
        model.CompanyId = companyId.Trim();
        model.Name = model.Name.ToUpper();
        model = await _warehouseService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(companyId, id);
        await _warehouseService.RemoveAsync(companyId, id);
        return Ok(warehouse);
    }
}
