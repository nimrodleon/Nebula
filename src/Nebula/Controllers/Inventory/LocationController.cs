using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class LocationController(
    IUserAuthenticationService userAuthenticationService,
    ILocationService locationService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Description" };
        var locations = await locationService.GetFilteredAsync(_companyId, fieldNames, query);
        return Ok(locations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var location = await locationService.GetByIdAsync(_companyId, id);
        return Ok(location);
    }

    [HttpGet("Stock/{id}")]
    public async Task<IActionResult> Stock(string id)
    {
        var respLocationDetailStock = await locationService.GetLocationDetailStocksAsync(_companyId, id);
        return Ok(respLocationDetailStock.LocationDetailStocks);
    }

    [HttpGet("Reponer/{ids}")]
    public async Task<IActionResult> Reponer(string ids)
    {
        var respLocationDetailStocks = new List<RespLocationDetailStock>();
        foreach (string id in ids.Split(","))
        {
            var respItem = await locationService.GetLocationDetailStocksAsync(_companyId, id, true);
            respLocationDetailStocks.Add(respItem);
        }

        return Ok(respLocationDetailStocks);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Location model)
    {
        model.CompanyId = _companyId.Trim();
        model.Description = model.Description.ToUpper();
        var location = await locationService.InsertOneAsync(model);
        return Ok(location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Location model)
    {
        var location = await locationService.GetByIdAsync(_companyId, id);
        model.Id = location.Id;
        model.CompanyId = _companyId.Trim();
        model.Description = model.Description.ToUpper();
        var responseData = await locationService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRole.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var location = await locationService.GetByIdAsync(_companyId, id);
        await locationService.DeleteOneAsync(_companyId, location.Id);
        return Ok(location);
    }

    [HttpGet("Warehouse/{id}")]
    public async Task<IActionResult> Warehouse(string id)
    {
        var locations = await locationService.GetByWarehouseIdAsync(_companyId, id);
        return Ok(locations);
    }
}
