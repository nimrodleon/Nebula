using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Dto.Inventory;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

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
        var location = await _locationService.GetByIdAsync(id);
        return Ok(location);
    }

    [HttpGet("Stock/{id}")]
    public async Task<IActionResult> Stock(string id)
    {
        var respLocationDetailStock = await _locationService.GetLocationDetailStocksAsync(id);
        return Ok(respLocationDetailStock.LocationDetailStocks);
    }

    [HttpGet("Reponer/{ids}")]
    public async Task<IActionResult> Reponer(string ids)
    {
        var respLocationDetailStocks = new List<RespLocationDetailStock>();
        foreach (string id in ids.Split(","))
        {
            var respItem = await _locationService.GetLocationDetailStocksAsync(id, true);
            respLocationDetailStocks.Add(respItem);
        }
        return Ok(respLocationDetailStocks);
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
        var location = await _locationService.GetByIdAsync(id);
        model.Id = location.Id;
        model.Description = model.Description.ToUpper();
        var responseData = await _locationService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var location = await _locationService.GetByIdAsync(id);
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
