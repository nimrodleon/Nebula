using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Models;

namespace Nebula.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class UserBusinessController(
    IUserService userService
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRegisterBusiness model)
    {
        try
        {
            var user = new User()
            {
                UserName = model.UserName,
                PasswordHash = PasswordHasher.HashPassword(model.Password.Trim()),
                Email = model.Email,
                AccountType = AccountTypeHelper.Business,
                UserRole = UserRoleDbHelper.Admin,
                FullName = model.FullName.ToUpper(),
                PhoneNumber = model.PhoneNumber,
                DefaultCompanyId = "-.-"
            };
            user = await userService.InsertOneAsync(user);
            return Ok(user);
        }
        catch (MongoWriteException ex)
            when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un usuario con el mismo nombre de usuario.");
        }
    }
}
