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
public class InventoryNotasController(
    IInventoryNotasService inventoryNotasService,
    IValidateStockService validateStockService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var responseData = await inventoryNotasService.GetListAsync(companyId, model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var inventoryNotas = await inventoryNotasService.GetByIdAsync(companyId, id);
        return Ok(inventoryNotas);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] InventoryNotas model)
    {
        model.CompanyId = companyId.Trim();
        var inventoryNotas = await inventoryNotasService.InsertOneAsync(model);
        return Ok(inventoryNotas);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] InventoryNotas model)
    {
        var inventoryNotas = await inventoryNotasService.GetByIdAsync(companyId, id);
        model.Id = inventoryNotas.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await inventoryNotasService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var inventoryNotas = await inventoryNotasService.GetByIdAsync(companyId, id);
        await inventoryNotasService.DeleteOneAsync(companyId, inventoryNotas.Id);
        return Ok(inventoryNotas);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string companyId, string id)
    {
        var inventoryNotas = await validateStockService.ValidarNotas(companyId, id);
        return Ok(inventoryNotas);
    }
}
