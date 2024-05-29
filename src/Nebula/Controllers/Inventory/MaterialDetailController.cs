using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class MaterialDetailController(
    IUserAuthenticationService userAuthenticationService,
    IMaterialDetailService materialDetailService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await materialDetailService.GetListAsync(_companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var materialDetail = await materialDetailService.GetByIdAsync(_companyId, id);
        return Ok(materialDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaterialDetail model)
    {
        model.CompanyId = _companyId.Trim();
        var materialDetail = await materialDetailService.InsertOneAsync(model);
        return Ok(materialDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] MaterialDetail model)
    {
        var materialDetail = await materialDetailService.GetByIdAsync(_companyId, id);
        model.Id = materialDetail.Id;
        model.CompanyId = _companyId.Trim();
        model.CreatedAt = materialDetail.CreatedAt;
        var responseData = await materialDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var materialDetail = await materialDetailService.GetByIdAsync(_companyId, id);
        await materialDetailService.DeleteOneAsync(_companyId, materialDetail.Id);
        return Ok(materialDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await materialDetailService.CountDocumentsAsync(_companyId, id);
        return Ok(countDocuments);
    }
}
