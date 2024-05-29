using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Notas;
using Nebula.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class InventoryNotasController(
    IUserAuthenticationService userAuthenticationService,
    IInventoryNotasService inventoryNotasService,
    IValidateStockService validateStockService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await inventoryNotasService.GetListAsync(_companyId, model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var inventoryNotas = await inventoryNotasService.GetByIdAsync(_companyId, id);
        return Ok(inventoryNotas);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryNotas model)
    {
        model.CompanyId = _companyId.Trim();
        var inventoryNotas = await inventoryNotasService.InsertOneAsync(model);
        return Ok(inventoryNotas);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] InventoryNotas model)
    {
        var inventoryNotas = await inventoryNotasService.GetByIdAsync(_companyId, id);
        model.Id = inventoryNotas.Id;
        model.CompanyId = _companyId.Trim();
        var responseData = await inventoryNotasService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRole.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var inventoryNotas = await inventoryNotasService.GetByIdAsync(_companyId, id);
        await inventoryNotasService.DeleteOneAsync(_companyId, inventoryNotas.Id);
        return Ok(inventoryNotas);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string id)
    {
        var inventoryNotas = await validateStockService.ValidarNotas(_companyId, id);
        return Ok(inventoryNotas);
    }
}
