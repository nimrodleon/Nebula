using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Models;

namespace Nebula.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("Index"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var roles = await _roleService.GetAsync("Nombre", query);
        return Ok(roles);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Show(string id)
    {
        var role = await _roleService.GetByIdAsync(id);
        return Ok(role);
    }

    [HttpPost("Create"), UserAuthorize(Permission.ConfigurationCreate)]
    public async Task<IActionResult> Create([FromBody] Roles model)
    {
        model.Nombre = model.Nombre.ToUpper();
        await _roleService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.ConfigurationEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] Roles model)
    {
        var role = await _roleService.GetByIdAsync(id);
        model.Id = role.Id;
        model.Nombre = model.Nombre.ToUpper();
        await _roleService.UpdateAsync(role.Id, model);
        return Ok(model);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.ConfigurationDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleService.GetByIdAsync(id);
        await _roleService.RemoveAsync(role.Id);
        return Ok(role);
    }
}
