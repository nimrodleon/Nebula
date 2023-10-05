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
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly IMaterialService _materialService;
    private readonly IValidateStockService _validateStockService;

    public MaterialController(
        IMaterialService materialService,
        IValidateStockService validateStockService)
    {
        _materialService = materialService;
        _validateStockService = validateStockService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var responseData = await _materialService.GetListAsync(companyId, model);
        return Ok(responseData);
    }

    [HttpGet("Contact/{id}")]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model, string id)
    {
        var responseData = await _materialService.GetListByContactIdAsync(companyId, model, id);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var location = await _materialService.GetByIdAsync(companyId, id);
        return Ok(location);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Material model)
    {
        model.CompanyId = companyId.Trim();
        var location = await _materialService.CreateAsync(model);
        return Ok(location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Material model)
    {
        var material = await _materialService.GetByIdAsync(id);
        model.Id = material.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await _materialService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var material = await _materialService.GetByIdAsync(companyId, id);
        await _materialService.RemoveAsync(companyId, material.Id);
        return Ok(material);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string companyId, string id)
    {
        var material = await _validateStockService.ValidarMaterial(companyId, id);
        return Ok(material);
    }
}
