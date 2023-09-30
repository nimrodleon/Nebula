using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Models;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Dto;

namespace Nebula.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRegister model)
    {
        var user = new User()
        {
            UserName = model.UserName,
            Email = model.Email,
            PasswordHash = PasswordHasher.HashPassword(model.Password),
            UserType = UserTypeSystem.Customer,
        };
        await _userService.CreateAsync(user);
        return Ok(user);
    }

    //[HttpGet]
    //public async Task<IActionResult> Index([FromQuery] string query = "")
    //{
    //    var users = await _userService.GetListAsync(query);
    //    return Ok(users);
    //}

    //[HttpGet("{id}")]
    //public async Task<IActionResult> Show(string id)
    //{
    //    var user = await _userService.GetByIdAsync(id);
    //    return Ok(user);
    //}

    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(string id, [FromBody] UserRegister model)
    //{
    //    var user = await _userService.GetByIdAsync(id);
    //    user.UserName = model.UserName;
    //    user.Email = model.Email;
    //    await _userService.UpdateAsync(id, user);
    //    return Ok(user);
    //}

    //[HttpPut("PasswordChange/{id}")]
    //public async Task<IActionResult> PasswordChange(string id, [FromBody] UserRegister model)
    //{
    //    var user = await _userService.GetByIdAsync(id);
    //    user.PasswordHash = PasswordHasher.HashPassword(model.Password);
    //    await _userService.UpdateAsync(id, user);
    //    return Ok(user);
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(string id)
    //{
    //    var user = await _userService.GetByIdAsync(id);
    //    await _userService.RemoveAsync(id);
    //    return Ok(user);
    //}
}
