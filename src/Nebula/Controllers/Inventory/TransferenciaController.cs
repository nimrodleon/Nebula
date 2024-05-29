using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Transferencias;
using Nebula.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class TransferenciaController(
    IUserAuthenticationService userAuthenticationService,
    ITransferenciaService transferenciaService,
    IValidateStockService validateStockService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await transferenciaService.GetListAsync(_companyId, model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var transferencia = await transferenciaService.GetByIdAsync(_companyId, id);
        return Ok(transferencia);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Transferencia model)
    {
        model.CompanyId = _companyId.Trim();
        var transferencia = await transferenciaService.InsertOneAsync(model);
        return Ok(transferencia);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Transferencia model)
    {
        var transferencia = await transferenciaService.GetByIdAsync(_companyId, id);
        model.Id = transferencia.Id;
        model.CompanyId = _companyId.Trim();
        var responseData = await transferenciaService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var transferencia = await transferenciaService.GetByIdAsync(_companyId, id);
        await transferenciaService.DeleteOneAsync(_companyId, transferencia.Id);
        return Ok(transferencia);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string id)
    {
        var transferencia = await validateStockService.ValidarTransferencia(_companyId, id);
        return Ok(transferencia);
    }
}
