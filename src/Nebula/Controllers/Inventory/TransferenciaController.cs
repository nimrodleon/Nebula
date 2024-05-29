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
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class TransferenciaController(
    ITransferenciaService transferenciaService,
    IValidateStockService validateStockService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var responseData = await transferenciaService.GetListAsync(companyId, model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var transferencia = await transferenciaService.GetByIdAsync(companyId, id);
        return Ok(transferencia);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Transferencia model)
    {
        model.CompanyId = companyId.Trim();
        var transferencia = await transferenciaService.InsertOneAsync(model);
        return Ok(transferencia);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Transferencia model)
    {
        var transferencia = await transferenciaService.GetByIdAsync(companyId, id);
        model.Id = transferencia.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await transferenciaService.ReplaceOneAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var transferencia = await transferenciaService.GetByIdAsync(companyId, id);
        await transferenciaService.DeleteOneAsync(companyId, transferencia.Id);
        return Ok(transferencia);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string companyId, string id)
    {
        var transferencia = await validateStockService.ValidarTransferencia(companyId, id);
        return Ok(transferencia);
    }
}
