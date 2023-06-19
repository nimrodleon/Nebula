using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Configurations;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Configurations.Subscriptions;

namespace Nebula.Controllers.Common;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class ConfigurationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationService _configurationService;
    private readonly ISubscriptionService _subscriptionService;

    public ConfigurationController(IConfiguration configuration,
        IConfigurationService configurationService,
        ISubscriptionService subscriptionService)
    {
        _configuration = configuration;
        _configurationService = configurationService;
        _subscriptionService = subscriptionService;
    }

    [HttpGet("Show")]
    public async Task<IActionResult> Show()
    {
        var configuration = await _configurationService.GetAsync();
        return Ok(configuration);
    }

    [HttpPut("Update"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Update([FromBody] Configuration model)
    {
        var configuration = await _configurationService.GetAsync();
        if (configuration is null) return NotFound();
        model.Id = configuration.Id;
        model = await _configurationService.UpdateAsync(model);
        return Ok(model);
    }

    [HttpGet("SincronizarPago")]
    public async Task<IActionResult> SincronizarPago()
    {
        var response = await _subscriptionService.SincronizarPago();
        return Ok(response);
    }

    [HttpGet("ValidarAcceso")]
    public async Task<IActionResult> ValidarAcceso()
    {
        var licenseDto = await _subscriptionService.ValidarAcceso();
        return Ok(licenseDto);
    }

    [HttpPatch("UpdateKey/{subscriptionId}")]
    public async Task<IActionResult> UpdateKey(string subscriptionId)
    {
        var configuration = await _subscriptionService.UpdateKey(subscriptionId);
        return Ok(configuration);
    }

    [HttpGet("FacturadorUrl")]
    [Obsolete]
    public Task<IActionResult> FacturadorUrl()
    {
        string? url = _configuration.GetValue<string>("facturadorUrl");
        return Task.FromResult<IActionResult>(Ok(new { url }));
    }

    [HttpGet("SearchPeUrl")]
    [Obsolete]
    public Task<IActionResult> SearchPeUrl()
    {
        string? url = _configuration.GetValue<string>("searchPeUrl");
        return Task.FromResult<IActionResult>(Ok(new { url }));
    }
}
