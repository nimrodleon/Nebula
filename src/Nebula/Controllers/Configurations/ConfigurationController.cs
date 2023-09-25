using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Configurations;
using Nebula.Modules.Configurations.Dto;
using Nebula.Modules.Configurations.Models;
using System.Security.Claims;

namespace Nebula.Controllers.Configurations;

[Route("api/[controller]")]
[ApiController]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationService _configurationService;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public ConfigurationController(
        IConfigurationService configurationService,
        IUserService userService, IRoleService roleService)
    {
        _configurationService = configurationService;
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

}
