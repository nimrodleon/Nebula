using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Configurations;
using Nebula.Modules.Configurations.Dto;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Configurations.Subscriptions;
using System.Security.Claims;

namespace Nebula.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationService _configurationService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public ConfigurationController(
        IConfigurationService configurationService,
        ISubscriptionService subscriptionService,
        IUserService userService, IRoleService roleService)
    {
        _configurationService = configurationService;
        _subscriptionService = subscriptionService;
        _userService = userService;
        _roleService = roleService;
    }

    [HttpGet("Show"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Show()
    {
        var configDto = new SystemConfigDto();
        configDto.Configuration = await _configurationService.GetAsync();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        configDto.User = await _userService.GetByIdAsync(userId);
        configDto.Roles = await _roleService.GetByIdAsync(configDto.User.RolesId);
        configDto.Configuration.AccessToken = string.Empty;
        configDto.User.PasswordHash = string.Empty;
        return Ok(configDto);
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
