using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Common;

namespace Nebula.Controllers.Common;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly CrudOperationService<Warehouse> _warehouseService;

    public WarehouseController(CrudOperationService<Warehouse> warehouseService) =>
        _warehouseService = warehouseService;

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _warehouseService.GetAsync("Name", query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        return Ok(warehouse);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Warehouse model)
    {
        model.Name = model.Name.ToUpper();
        await _warehouseService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Warehouse model)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);

        model.Id = warehouse.Id;
        model.Name = model.Name.ToUpper();
        model = await _warehouseService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        await _warehouseService.RemoveAsync(id);
        return Ok(warehouse);
    }
}
