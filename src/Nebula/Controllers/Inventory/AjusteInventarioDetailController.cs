using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class AjusteInventarioDetailController(IAjusteInventarioDetailService ajusteInventarioDetailService)
    : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var responseData = await ajusteInventarioDetailService.GetListAsync(companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var ajusteInventarioDetail = await ajusteInventarioDetailService.GetByIdAsync(companyId, id);
        return Ok(ajusteInventarioDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] AjusteInventarioDetail model)
    {
        model.CompanyId = companyId.Trim();
        var ajusteInventarioDetail = await ajusteInventarioDetailService.InsertOneAsync(model);
        return Ok(ajusteInventarioDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] AjusteInventarioDetail model)
    {
        var ajusteInventarioDetail = await ajusteInventarioDetailService.GetByIdAsync(companyId, id);
        model.Id = ajusteInventarioDetail.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await ajusteInventarioDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var ajusteInventarioDetail = await ajusteInventarioDetailService.GetByIdAsync(companyId, id);
        await ajusteInventarioDetailService.DeleteOneAsync(companyId, ajusteInventarioDetail.Id);
        return Ok(ajusteInventarioDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await ajusteInventarioDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }
}
