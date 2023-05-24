using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Transferencias;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Dto;
using Nebula.Modules.Configurations.Subscriptions;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class TransferenciaController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ITransferenciaService _transferenciaService;
    private readonly IValidateStockService _validateStockService;

    public TransferenciaController(ISubscriptionService subscriptionService,
        ITransferenciaService transferenciaService,
        IValidateStockService validateStockService)
    {
        _subscriptionService = subscriptionService;
        _transferenciaService = transferenciaService;
        _validateStockService = validateStockService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await _transferenciaService.GetListAsync(model);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(id);
        return Ok(transferencia);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Transferencia model)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var transferencia = await _transferenciaService.CreateAsync(model);
        return Ok(transferencia);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Transferencia model)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var transferencia = await _transferenciaService.GetByIdAsync(id);
        model.Id = transferencia.Id;
        var responseData = await _transferenciaService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(id);
        await _transferenciaService.RemoveAsync(transferencia.Id);
        return Ok(transferencia);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string id)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var transferencia = await _validateStockService.ValidarTransferencia(id);
        return Ok(transferencia);
    }
}
