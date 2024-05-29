using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class AjusteInventarioDetailController(
    IUserAuthenticationService userAuthenticationService,
    IAjusteInventarioDetailService ajusteInventarioDetailService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
        var responseData = await ajusteInventarioDetailService.GetListAsync(_companyId, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var ajusteInventarioDetail = await ajusteInventarioDetailService.GetByIdAsync(_companyId, id);
        return Ok(ajusteInventarioDetail);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AjusteInventarioDetail model)
    {
        model.CompanyId = _companyId.Trim();
        var ajusteInventarioDetail = await ajusteInventarioDetailService.InsertOneAsync(model);
        return Ok(ajusteInventarioDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AjusteInventarioDetail model)
    {
        var ajusteInventarioDetail = await ajusteInventarioDetailService.GetByIdAsync(_companyId, id);
        model.Id = ajusteInventarioDetail.Id;
        model.CompanyId = _companyId.Trim();
        var responseData = await ajusteInventarioDetailService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var ajusteInventarioDetail = await ajusteInventarioDetailService.GetByIdAsync(_companyId, id);
        await ajusteInventarioDetailService.DeleteOneAsync(_companyId, ajusteInventarioDetail.Id);
        return Ok(ajusteInventarioDetail);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await ajusteInventarioDetailService.CountDocumentsAsync(_companyId, id);
        return Ok(countDocuments);
    }
}
