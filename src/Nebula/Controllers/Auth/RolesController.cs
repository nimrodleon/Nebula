using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Models;

namespace Nebula.Controllers.Auth;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var roles = await _roleService.GetAsync("Nombre", query);
        return Ok(roles);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var role = await _roleService.GetByIdAsync(id);
        return Ok(role);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Roles model)
    {
        model.Nombre = model.Nombre.ToUpper();
        await _roleService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Roles model)
    {
        var role = await _roleService.GetByIdAsync(id);
        model.Id = role.Id;
        model.Nombre = model.Nombre.ToUpper();
        await _roleService.UpdateAsync(role.Id, model);
        return Ok(model);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleService.GetByIdAsync(id);
        await _roleService.RemoveAsync(role.Id);
        return Ok(role);
    }
}
