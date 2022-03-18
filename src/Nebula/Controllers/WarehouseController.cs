using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Models;
using Nebula.Data.Services;

namespace Nebula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly WarehouseService _warehouseService;

    public WarehouseController(WarehouseService warehouseService) =>
        _warehouseService = warehouseService;

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _warehouseService.GetListAsync(query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var warehouse = await _warehouseService.GetAsync(id);
        if (warehouse is null) return NotFound();
        return Ok(warehouse);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Warehouse model)
    {
        model.Name = model.Name.ToUpper();
        await _warehouseService.CreateAsync(model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"El Almacén {model.Name} ha sido registrado!"
        });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Warehouse model)
    {
        var warehouse = await _warehouseService.GetAsync(id);
        if (warehouse is null) return NotFound();

        model.Id = warehouse.Id;
        model.Name = model.Name.ToUpper();
        await _warehouseService.UpdateAsync(id, model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"El Almacén {model.Name} ha sido actualizado!"
        });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var warehouse = await _warehouseService.GetAsync(id);
        if (warehouse is null) return NotFound();
        await _warehouseService.RemoveAsync(id);
        return Ok(new { Ok = true, Data = warehouse, Msg = "El almacén ha sido borrado!" });
    }
}
