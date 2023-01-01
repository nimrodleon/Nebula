using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Services.Inventory;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly LocationService _locationService;

    public LocationController(LocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _locationService.GetAsync("Description", query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var location = await _locationService.GetAsync(id);
        return Ok(location);
    }

    [HttpGet("Stock/{id}")]
    public async Task<IActionResult> Stock(string id)
    {
        var locationDetailStockDtos = await _locationService.GetLocationDetailStocksAsync(id);
        return Ok(locationDetailStockDtos);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Location model)
    {
        model.Description = model.Description.ToUpper();
        var location = await _locationService.CreateAsync(model);
        return Ok(location);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Location model)
    {
        var location = await _locationService.GetAsync(id);
        model.Id = location.Id;
        model.Description = model.Description.ToUpper();
        var responseData = await _locationService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var location = await _locationService.GetAsync(id);
        await _locationService.RemoveAsync(location.Id);
        return Ok(location);
    }

    [HttpGet("Warehouse/{id}")]
    public async Task<IActionResult> Warehouse(string id)
    {
        var locations = await _locationService.GetByWarehouseIdAsync(id);
        return Ok(locations);
    }
}
