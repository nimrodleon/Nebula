using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Configurations;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Configurations.Subscriptions;

namespace Nebula.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationService _configurationService;
    private readonly ISubscriptionService _subscriptionService;

    public ConfigurationController(
        IConfigurationService configurationService,
        ISubscriptionService subscriptionService)
    {
        _configurationService = configurationService;
        _subscriptionService = subscriptionService;
    }

    [HttpGet("Show"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Show()
    {
        var configuration = await _configurationService.GetAsync();
        return Ok(configuration);
    }

    [HttpPut("Update"), UserAuthorize(Permission.ConfigurationEdit)]
    public async Task<IActionResult> Update([FromBody] Configuration model)
    {
        var configuration = await _configurationService.GetAsync();
        if (configuration is null) return NotFound();
        model.Id = configuration.Id;
        model = await _configurationService.UpdateAsync(model);
        return Ok(model);
    }

    [HttpGet("SincronizarPago"), AllowAnonymous]
    public async Task<IActionResult> SincronizarPago()
    {
        var response = await _subscriptionService.SincronizarPago();
        return Ok(response);
    }

    [HttpGet("ValidarAcceso"), AllowAnonymous]
    public async Task<IActionResult> ValidarAcceso()
    {
        var licenseDto = await _subscriptionService.ValidarAcceso();
        return Ok(licenseDto);
    }

    [HttpPatch("UpdateKey/{subscriptionId}"), UserAuthorize(Permission.ConfigurationEdit)]
    public async Task<IActionResult> UpdateKey(string subscriptionId)
    {
        var configuration = await _subscriptionService.UpdateKey(subscriptionId);
        return Ok(configuration);
    }

}
