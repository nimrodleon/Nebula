using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Transferencias;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Dto;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Inventory;

[Route("api/[controller]")]
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

    [HttpGet("Index"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await _transferenciaService.GetListAsync(model);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.InventoryRead)]
    public async Task<IActionResult> Show(string id)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(id);
        return Ok(transferencia);
    }

    [HttpPost("Create"), UserAuthorize(Permission.InventoryCreate)]
    public async Task<IActionResult> Create([FromBody] Transferencia model)
    {
        var transferencia = await _transferenciaService.CreateAsync(model);
        return Ok(transferencia);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] Transferencia model)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(id);
        model.Id = transferencia.Id;
        var responseData = await _transferenciaService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.InventoryDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(id);
        await _transferenciaService.RemoveAsync(transferencia.Id);
        return Ok(transferencia);
    }

    [HttpGet("Validate/{id}"), UserAuthorize(Permission.InventoryEdit)]
    public async Task<IActionResult> Validate(string id)
    {
        var transferencia = await _validateStockService.ValidarTransferencia(id);
        return Ok(transferencia);
    }
}
