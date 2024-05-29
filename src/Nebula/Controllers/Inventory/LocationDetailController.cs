using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class LocationDetailController(ILocationDetailService locationDetailService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var responseData = await locationDetailService.GetListAsync(companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var locationDetail = await locationDetailService.GetByIdAsync(companyId, id);
        return Ok(locationDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] LocationDetail model)
    {
        model.CompanyId = companyId.Trim();
        var locationDetail = await locationDetailService.InsertOneAsync(model);
        return Ok(locationDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] LocationDetail model)
    {
        var locationDetail = await locationDetailService.GetByIdAsync(companyId, id);
        model.Id = locationDetail.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await locationDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var locationDetail = await locationDetailService.GetByIdAsync(companyId, id);
        await locationDetailService.DeleteOneAsync(companyId, locationDetail.Id);
        return Ok(locationDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await locationDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }
}
