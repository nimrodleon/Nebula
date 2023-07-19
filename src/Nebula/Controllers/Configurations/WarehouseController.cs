using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Configurations.Warehouses;

namespace Nebula.Controllers.Configurations;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService) =>
        _warehouseService = warehouseService;

    [HttpGet("Index"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _warehouseService.GetAsync("Name", query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Show(string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        return Ok(warehouse);
    }

    [HttpPost("Create"), UserAuthorize(Permission.ConfigurationCreate)]
    public async Task<IActionResult> Create([FromBody] Warehouse model)
    {
        model.Name = model.Name.ToUpper();
        await _warehouseService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.ConfigurationEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] Warehouse model)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);

        model.Id = warehouse.Id;
        model.Name = model.Name.ToUpper();
        model = await _warehouseService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.ConfigurationDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        await _warehouseService.RemoveAsync(id);
        return Ok(warehouse);
    }
}
