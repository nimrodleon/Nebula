using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Helpers;
using Nebula.Data.Models;
using Nebula.Data.Services;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService) =>
        _userService = userService;

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _userService.GetListAsync(query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var user = await _userService.GetAsync(id);
        return Ok(user);
    }

    [HttpPost("Create")]
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

        return Ok(new { Ok = true, User = user });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UserRegister model)
    {
        var user = await _userService.GetAsync(id);
        user.UserName = model.UserName;
        user.Email = model.Email;
        user.Role = model.Role;
        await _userService.UpdateAsync(id, user);

        return Ok(new
        {
            Ok = true,
            Data = user,
            Msg = $"El usuario {user.UserName} ha sido actualizado!"
        });
    }

    [HttpPut("PasswordChange/{id}")]
    public async Task<IActionResult> PasswordChange(string id, [FromBody] UserRegister model)
    {
        var user = await _userService.GetAsync(id);
        user.PasswordHash = PasswordHasher.HashPassword(model.Password);
        await _userService.UpdateAsync(id, user);

        return Ok(new
        {
            Ok = true,
            Data = user,
            Msg = $"La contraseña de {user.UserName} ha sido actualizado!"
        });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetAsync(id);
        await _userService.RemoveAsync(id);
        return Ok(new { Ok = true, Data = user, Msg = "El usuario ha sido borrado!" });
    }
}
