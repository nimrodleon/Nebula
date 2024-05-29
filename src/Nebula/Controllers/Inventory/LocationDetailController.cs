using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class LocationDetailController(
    IUserAuthenticationService userAuthenticationService,
    ILocationDetailService locationDetailService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await locationDetailService.GetListAsync(_companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var locationDetail = await locationDetailService.GetByIdAsync(_companyId, id);
        return Ok(locationDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LocationDetail model)
    {
        model.CompanyId = _companyId.Trim();
        var locationDetail = await locationDetailService.InsertOneAsync(model);
        return Ok(locationDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] LocationDetail model)
    {
        var locationDetail = await locationDetailService.GetByIdAsync(_companyId, id);
        model.Id = locationDetail.Id;
        model.CompanyId = _companyId.Trim();
        var responseData = await locationDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var locationDetail = await locationDetailService.GetByIdAsync(_companyId, id);
        await locationDetailService.DeleteOneAsync(_companyId, locationDetail.Id);
        return Ok(locationDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await locationDetailService.CountDocumentsAsync(_companyId, id);
        return Ok(countDocuments);
    }
}
