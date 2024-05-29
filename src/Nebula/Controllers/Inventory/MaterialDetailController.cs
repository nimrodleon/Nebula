using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class MaterialDetailController(IMaterialDetailService materialDetailService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var responseData = await materialDetailService.GetListAsync(companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var materialDetail = await materialDetailService.GetByIdAsync(companyId, id);
        return Ok(materialDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] MaterialDetail model)
    {
        model.CompanyId = companyId.Trim();
        var materialDetail = await materialDetailService.InsertOneAsync(model);
        return Ok(materialDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] MaterialDetail model)
    {
        var materialDetail = await materialDetailService.GetByIdAsync(companyId, id);
        model.Id = materialDetail.Id;
        model.CompanyId = companyId.Trim();
        model.CreatedAt = materialDetail.CreatedAt;
        var responseData = await materialDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var materialDetail = await materialDetailService.GetByIdAsync(companyId, id);
        await materialDetailService.DeleteOneAsync(companyId, materialDetail.Id);
        return Ok(materialDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await materialDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }
}
