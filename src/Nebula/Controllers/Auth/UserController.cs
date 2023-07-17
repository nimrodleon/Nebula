using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Models;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Dto;

namespace Nebula.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet("Index"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _userService.GetListAsync(query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Show(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpPost("Create"), UserAuthorize(Permission.ConfigurationCreate)]
    public async Task<IActionResult> Create([FromBody] UserRegister model)
    {
        var user = new User()
        {
            UserName = model.UserName,
            Email = model.Email,
            PasswordHash = PasswordHasher.HashPassword(model.Password),
            RolesId = model.RolesId
        };
        await _userService.CreateAsync(user);
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("CreateSupportUser")]
    public async Task<IActionResult> CreateSupportUser()
    {
        var createUser = _configuration.GetValue<bool>("CreateSupportUser");
        if (!createUser) return NotFound();

        var existingUser = await _userService.GetByUserNameAsync("soporte");
        if (existingUser != null) await _userService.RemoveAsync(existingUser.Id);

        var user = new User()
        {
            UserName = "soporte",
            Email = "soporte@local.pe",
            PasswordHash = PasswordHasher.HashPassword("mangoloco"),
            Role = AuthRoles.Admin
        };
        await _userService.CreateAsync(user);
        return Ok(user);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.ConfigurationEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] UserRegister model)
    {
        var user = await _userService.GetByIdAsync(id);
        user.UserName = model.UserName;
        user.Email = model.Email;
        user.RolesId = model.RolesId;
        await _userService.UpdateAsync(id, user);
        return Ok(user);
    }

    [HttpPut("PasswordChange/{id}"), UserAuthorize(Permission.ConfigurationEdit)]
    public async Task<IActionResult> PasswordChange(string id, [FromBody] UserRegister model)
    {
        var user = await _userService.GetByIdAsync(id);
        user.PasswordHash = PasswordHasher.HashPassword(model.Password);
        await _userService.UpdateAsync(id, user);
        return Ok(user);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.ConfigurationDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        await _userService.RemoveAsync(id);
        return Ok(user);
    }
}
