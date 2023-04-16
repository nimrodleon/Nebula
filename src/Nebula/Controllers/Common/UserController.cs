using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Common;
using Nebula.Database.Services.Common;
using Nebula.Database.Dto.Common;

namespace Nebula.Controllers.Common;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _userService.GetListAsync(query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpPost("Create"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Create([FromBody] UserRegister model)
    {
        var user = new User()
        {
            UserName = model.UserName,
            Email = model.Email,
            PasswordHash = PasswordHasher.HashPassword(model.Password),
            Role = model.Role
        };
        await _userService.CreateAsync(user);
        return Ok(user);
    }

    /// <summary>
    /// Crear usuario con clave maestra.
    /// </summary>
    /// <param name="masterKey">Hash clave maestra</param>
    /// <param name="model">Datos del usuario</param>
    /// <returns>User</returns>
    [AllowAnonymous]
    [HttpPost("CreateUser/{masterKey}")]
    public async Task<IActionResult> CreateUserAdmin(string masterKey, [FromBody] UserRegister model)
    {
        string hash = MasterKeyDto.ReadHashFile();
        if (hash != masterKey) return NotFound();

        var user = new User()
        {
            UserName = model.UserName.Trim(),
            Email = model.Email.Trim(),
            PasswordHash = PasswordHasher.HashPassword(model.Password.Trim()),
            Role = model.Role
        };
        await _userService.CreateAsync(user);
        return Ok(user);
    }

    [HttpPut("Update/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Update(string id, [FromBody] UserRegister model)
    {
        var user = await _userService.GetByIdAsync(id);
        user.UserName = model.UserName;
        user.Email = model.Email;
        user.Role = model.Role;
        await _userService.UpdateAsync(id, user);
        return Ok(user);
    }

    [HttpPut("PasswordChange/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> PasswordChange(string id, [FromBody] UserRegister model)
    {
        var user = await _userService.GetByIdAsync(id);
        user.PasswordHash = PasswordHasher.HashPassword(model.Password);
        await _userService.UpdateAsync(id, user);
        return Ok(user);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        await _userService.RemoveAsync(id);
        return Ok(user);
    }
}
