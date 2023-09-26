using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Transferencias;
using Nebula.Common.Dto;
using Microsoft.AspNetCore.Authorization;

namespace Nebula.Controllers.Inventory;

[Authorize]
[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class TransferenciaController : ControllerBase
{
    private readonly ITransferenciaService _transferenciaService;
    private readonly IValidateStockService _validateStockService;

    public TransferenciaController(
        ITransferenciaService transferenciaService,
        IValidateStockService validateStockService)
    {
        _transferenciaService = transferenciaService;
        _validateStockService = validateStockService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var responseData = await _transferenciaService.GetListAsync(companyId, model);
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(companyId, id);
        return Ok(transferencia);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Transferencia model)
    {
        model.CompanyId = companyId.Trim();
        var transferencia = await _transferenciaService.CreateAsync(model);
        return Ok(transferencia);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Transferencia model)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(companyId, id);
        model.Id = transferencia.Id;
        model.CompanyId = companyId.Trim();
        var responseData = await _transferenciaService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(companyId, id);
        await _transferenciaService.RemoveAsync(companyId, transferencia.Id);
        return Ok(transferencia);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string companyId, string id)
    {
        var transferencia = await _validateStockService.ValidarTransferencia(companyId, id);
        return Ok(transferencia);
    }
}
