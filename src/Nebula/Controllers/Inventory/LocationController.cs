using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Description" };
        var locations = await _locationService.GetFilteredAsync(companyId, fieldNames, query);
        return Ok(locations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var location = await _locationService.GetByIdAsync(companyId, id);
        return Ok(location);
    }

    [HttpGet("Stock/{id}")]
    public async Task<IActionResult> Stock(string companyId, string id)
    {
        var respLocationDetailStock = await _locationService.GetLocationDetailStocksAsync(companyId, id);
        return Ok(respLocationDetailStock.LocationDetailStocks);
    }

    [HttpGet("Reponer/{ids}")]
    public async Task<IActionResult> Reponer(string companyId, string ids)
    {
        var respLocationDetailStocks = new List<RespLocationDetailStock>();
        foreach (string id in ids.Split(","))
        {
            var respItem = await _locationService.GetLocationDetailStocksAsync(companyId, id, true);
            respLocationDetailStocks.Add(respItem);
        }
        return Ok(respLocationDetailStocks);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Location model)
    {
        model.CompanyId = companyId.Trim();
        model.Description = model.Description.ToUpper();
        var location = await _locationService.CreateAsync(model);
        return Ok(location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Location model)
    {
        var location = await _locationService.GetByIdAsync(companyId, id);
        model.Id = location.Id;
        model.CompanyId = companyId.Trim();
        model.Description = model.Description.ToUpper();
        var responseData = await _locationService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var location = await _locationService.GetByIdAsync(companyId, id);
        await _locationService.RemoveAsync(companyId, location.Id);
        return Ok(location);
    }

    [HttpGet("Warehouse/{id}")]
    public async Task<IActionResult> Warehouse(string companyId, string id)
    {
        var locations = await _locationService.GetByWarehouseIdAsync(companyId, id);
        return Ok(locations);
    }
}
