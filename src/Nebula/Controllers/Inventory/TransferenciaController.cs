using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Services.Inventory;
using Nebula.Database.Dto.Common;
using Nebula.Database.Services.Common;
using Nebula.Modules.Inventory.Stock;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class TransferenciaController : ControllerBase
{
    private readonly TransferenciaService _transferenciaService;
    private readonly ValidateStockService _validateStockService;
    private readonly ConfigurationService _configurationService;

    public TransferenciaController(TransferenciaService transferenciaService,
        ValidateStockService validateStockService, ConfigurationService configurationService)
    {
        _transferenciaService = transferenciaService;
        _validateStockService = validateStockService;
        _configurationService = configurationService;
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
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var transferencia = await _transferenciaService.CreateAsync(model);
        return Ok(transferencia);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Transferencia model)
    {
        var license = await _configurationService.ValidarAcceso();
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
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var transferencia = await _validateStockService.ValidarTransferencia(id);
        return Ok(transferencia);
    }
}
