using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Notas;
using Nebula.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class InventoryNotasController : ControllerBase
{
    private readonly IInventoryNotasService _inventoryNotasService;
    private readonly IValidateStockService _validateStockService;

    public InventoryNotasController(
        IInventoryNotasService inventoryNotasService,
        IValidateStockService validateStockService)
    {
        _inventoryNotasService = inventoryNotasService;
        _validateStockService = validateStockService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var responseData = await _inventoryNotasService.GetListAsync(companyId, model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(companyId, id);
        return Ok(inventoryNotas);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] InventoryNotas model)
    {
        model.CompanyId = companyId.Trim();
        var inventoryNotas = await _inventoryNotasService.CreateAsync(model);
        return Ok(inventoryNotas);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] InventoryNotas model)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(companyId, id);
        model.Id = inventoryNotas.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await _inventoryNotasService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(companyId, id);
        await _inventoryNotasService.RemoveAsync(companyId, inventoryNotas.Id);
        return Ok(inventoryNotas);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string companyId, string id)
    {
        var inventoryNotas = await _validateStockService.ValidarNotas(companyId, id);
        return Ok(inventoryNotas);
    }
}
