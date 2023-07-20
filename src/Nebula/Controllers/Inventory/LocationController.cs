using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet("Index"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _locationService.GetAsync("Description", query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Show(string id)
    {
        var location = await _locationService.GetByIdAsync(id);
        return Ok(location);
    }

    [HttpGet("Stock/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Stock(string id)
    {
        var respLocationDetailStock = await _locationService.GetLocationDetailStocksAsync(id);
        return Ok(respLocationDetailStock.LocationDetailStocks);
    }

    [HttpGet("Reponer/{ids}"), UserAuthorize(Permission.InventoryRead)]
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

    [HttpPost("Create"), UserAuthorize(Permission.InventoryCreate)]
    public async Task<IActionResult> Create([FromBody] Location model)
    {
        model.Description = model.Description.ToUpper();
        var location = await _locationService.CreateAsync(model);
        return Ok(location);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] Location model)
    {
        var location = await _locationService.GetByIdAsync(id);
        model.Id = location.Id;
        model.Description = model.Description.ToUpper();
        var responseData = await _locationService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.InventoryDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var location = await _locationService.GetByIdAsync(id);
        await _locationService.RemoveAsync(location.Id);
        return Ok(location);
    }

    [HttpGet("Warehouse/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Warehouse(string id)
    {
        var locations = await _locationService.GetByWarehouseIdAsync(id);
        return Ok(locations);
    }
}
