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
public class AjusteInventarioController : ControllerBase
{
    private readonly IAjusteInventarioService _ajusteInventarioService;
    private readonly IAjusteInventarioDetailService _ajusteInventarioDetailService;
    private readonly IValidateStockService _validateStockService;

    public AjusteInventarioController(
        IAjusteInventarioService ajusteInventarioService,
        IAjusteInventarioDetailService ajusteInventarioDetailService,
        IValidateStockService validateStockService)
    {
        _ajusteInventarioService = ajusteInventarioService;
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
        _validateStockService = validateStockService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var responseData = await _ajusteInventarioService.GetListAsync(companyId ,model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(companyId, id);
        return Ok(ajusteInventario);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] AjusteInventario model)
    {
        model.CompanyId = companyId.Trim();
        var ajusteInventario = await _ajusteInventarioService.CreateAsync(model);
        await _ajusteInventarioDetailService.GenerateDetailAsync(companyId, ajusteInventario.LocationId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] AjusteInventario model)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(companyId, id);
        model.Id = ajusteInventario.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await _ajusteInventarioService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(companyId, id);
        await _ajusteInventarioService.RemoveAsync(companyId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string companyId, string id)
    {
        var ajusteInventario = await _validateStockService.ValidarAjusteInventario(companyId, id);
        return Ok(ajusteInventario);
    }
}
