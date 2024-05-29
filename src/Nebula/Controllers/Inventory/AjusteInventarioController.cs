using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class AjusteInventarioController(
    IUserAuthenticationService userAuthenticationService,
    IAjusteInventarioService ajusteInventarioService,
    IAjusteInventarioDetailService ajusteInventarioDetailService,
    IValidateStockService validateStockService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await ajusteInventarioService.GetListAsync(_companyId, model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var ajusteInventario = await ajusteInventarioService.GetByIdAsync(_companyId, id);
        return Ok(ajusteInventario);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AjusteInventario model)
    {
        model.CompanyId = _companyId.Trim();
        var ajusteInventario = await ajusteInventarioService.InsertOneAsync(model);
        await ajusteInventarioDetailService.GenerateDetailAsync(_companyId, ajusteInventario.LocationId,
            ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AjusteInventario model)
    {
        var ajusteInventario = await ajusteInventarioService.GetByIdAsync(_companyId, id);
        model.Id = ajusteInventario.Id;
        model.CompanyId = _companyId.Trim();
        var responseData = await ajusteInventarioService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRole.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var ajusteInventario = await ajusteInventarioService.GetByIdAsync(_companyId, id);
        await ajusteInventarioService.DeleteOneAsync(_companyId, ajusteInventario.Id);
        return Ok(ajusteInventario);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string id)
    {
        var ajusteInventario = await validateStockService.ValidarAjusteInventario(_companyId, id);
        return Ok(ajusteInventario);
    }
}
