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
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class MaterialController(
    IMaterialService materialService,
    IValidateStockService validateStockService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var responseData = await materialService.GetListAsync(companyId, model);
        return Ok(responseData);
    }

    [HttpGet("Contact/{id}")]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model, string id)
    {
        var responseData = await materialService.GetListByContactIdAsync(companyId, model, id);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var location = await materialService.GetByIdAsync(companyId, id);
        return Ok(location);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Material model)
    {
        model.CompanyId = companyId.Trim();
        var location = await materialService.InsertOneAsync(model);
        return Ok(location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Material model)
    {
        var material = await materialService.GetByIdAsync(id);
        model.Id = material.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await materialService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var material = await materialService.GetByIdAsync(companyId, id);
        await materialService.DeleteOneAsync(companyId, material.Id);
        return Ok(material);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string companyId, string id)
    {
        var material = await validateStockService.ValidarMaterial(companyId, id);
        return Ok(material);
    }
}
