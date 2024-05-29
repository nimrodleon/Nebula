using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class AjusteInventarioController(
    IAjusteInventarioService ajusteInventarioService,
    IAjusteInventarioDetailService ajusteInventarioDetailService,
    IValidateStockService validateStockService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var responseData = await ajusteInventarioService.GetListAsync(companyId ,model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var ajusteInventario = await ajusteInventarioService.GetByIdAsync(companyId, id);
        return Ok(ajusteInventario);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] AjusteInventario model)
    {
        model.CompanyId = companyId.Trim();
        var ajusteInventario = await ajusteInventarioService.InsertOneAsync(model);
        await ajusteInventarioDetailService.GenerateDetailAsync(companyId, ajusteInventario.LocationId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] AjusteInventario model)
    {
        var ajusteInventario = await ajusteInventarioService.GetByIdAsync(companyId, id);
        model.Id = ajusteInventario.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await ajusteInventarioService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var ajusteInventario = await ajusteInventarioService.GetByIdAsync(companyId, id);
        await ajusteInventarioService.DeleteOneAsync(companyId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string companyId, string id)
    {
        var ajusteInventario = await validateStockService.ValidarAjusteInventario(companyId, id);
        return Ok(ajusteInventario);
    }
}
