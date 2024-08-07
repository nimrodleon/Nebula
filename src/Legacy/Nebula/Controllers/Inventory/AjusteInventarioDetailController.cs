using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class AjusteInventarioDetailController : ControllerBase
{
    private readonly IAjusteInventarioDetailService _ajusteInventarioDetailService;

    public AjusteInventarioDetailController(IAjusteInventarioDetailService ajusteInventarioDetailService)
    {
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id)
    {
        var responseData = await _ajusteInventarioDetailService.GetListAsync(companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var ajusteInventarioDetail = await _ajusteInventarioDetailService.GetByIdAsync(companyId, id);
        return Ok(ajusteInventarioDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] AjusteInventarioDetail model)
    {
        model.CompanyId = companyId.Trim();
        var ajusteInventarioDetail = await _ajusteInventarioDetailService.CreateAsync(model);
        return Ok(ajusteInventarioDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] AjusteInventarioDetail model)
    {
        var ajusteInventarioDetail = await _ajusteInventarioDetailService.GetByIdAsync(companyId, id);
        model.Id = ajusteInventarioDetail.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await _ajusteInventarioDetailService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var ajusteInventarioDetail = await _ajusteInventarioDetailService.GetByIdAsync(companyId, id);
        await _ajusteInventarioDetailService.RemoveAsync(companyId, ajusteInventarioDetail.Id);
        return Ok(ajusteInventarioDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await _ajusteInventarioDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }
}
