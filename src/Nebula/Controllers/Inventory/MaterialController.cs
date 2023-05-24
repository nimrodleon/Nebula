using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Dto;
using Nebula.Modules.Configurations.Subscriptions;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IMaterialService _materialService;
    private readonly IValidateStockService _validateStockService;

    public MaterialController(ISubscriptionService subscriptionService,
        IMaterialService materialService,
        IValidateStockService validateStockService)
    {
        _subscriptionService = subscriptionService;
        _materialService = materialService;
        _validateStockService = validateStockService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var responseData = await _materialService.GetListAsync(model);
        return Ok(responseData);
    }

    [HttpGet("Contact/{id}")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model, string id)
    {
        var responseData = await _materialService.GetListByContactIdAsync(model, id);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var location = await _materialService.GetByIdAsync(id);
        return Ok(location);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Material model)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var location = await _materialService.CreateAsync(model);
        return Ok(location);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Material model)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var material = await _materialService.GetByIdAsync(id);
        model.Id = material.Id;
        var responseData = await _materialService.UpdateAsync(id, model);
        return Ok(responseData);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var material = await _materialService.GetByIdAsync(id);
        await _materialService.RemoveAsync(material.Id);
        return Ok(material);
    }

    [HttpGet("Validate/{id}")]
    public async Task<IActionResult> Validate(string id)
    {
        var license = await _subscriptionService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        var material = await _validateStockService.ValidarMaterial(id);
        return Ok(material);
    }
}
