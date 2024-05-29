using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class MaterialController(
    IUserAuthenticationService userAuthenticationService,
    IMaterialService materialService,
    IValidateStockService validateStockService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await materialService.GetListAsync(_companyId, model);
        return Ok(responseData);
    }

    [HttpGet("Contact/{id}")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model, string id)
    {
        var responseData = await materialService.GetListByContactIdAsync(_companyId, model, id);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var location = await materialService.GetByIdAsync(_companyId, id);
        return Ok(location);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Material model)
    {
        model.CompanyId = _companyId.Trim();
        var location = await materialService.InsertOneAsync(model);
        return Ok(location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Material model)
    {
        var material = await materialService.GetByIdAsync(id);
        model.Id = material.Id;
        model.CompanyId = _companyId.Trim();
        var responseData = await materialService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var material = await materialService.GetByIdAsync(_companyId, id);
        await materialService.DeleteOneAsync(_companyId, material.Id);
        return Ok(material);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string id)
    {
        var material = await validateStockService.ValidarMaterial(_companyId, id);
        return Ok(material);
    }
}
