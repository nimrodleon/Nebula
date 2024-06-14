using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Nebula.Common.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Models;

namespace Nebula.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class UserPersonalController(
    IUserAuthenticationService userAuthenticationService,
    IUserService userService
) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var usuarios = await userService.GetListAsync(_companyId, query, page, pageSize);
        var totalUsuarios = await userService.GetTotalListAsync(_companyId, query);
        var totalPages = (int)Math.Ceiling((double)totalUsuarios / pageSize);

        var paginationInfo = new PaginationInfo()
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<User>
        {
            Pagination = paginationInfo,
            Data = usuarios
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRegisterPersonal model)
    {
        try
        {
            string secret = Guid.NewGuid().ToString();
            var user = new User()
            {
                UserName = model.UserName,
                PasswordHash = PasswordHasher.HashPassword(secret),
                Email = model.Email,
                AccountType = AccountTypeHelper.Personal,
                UserRole = model.UserRole,
                FullName = model.FullName.ToUpper(),
                PhoneNumber = model.PhoneNumber,
                DefaultCompanyId = _companyId.Trim()
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UserRegisterPersonal model)
    {
        try
        {
            var user = await userService.GetByIdAsync(id);
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.AccountType = AccountTypeHelper.Personal;
            user.UserRole = model.UserRole;
            user.FullName = model.FullName.ToUpper();
            user.PhoneNumber = model.PhoneNumber;
            user.DefaultCompanyId = _companyId.Trim();
            user = await userService.ReplaceOneAsync(user.Id, user);
            return Ok(user);
        }
        catch (MongoWriteException ex)
            when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un usuario con el mismo nombre de usuario.");
        }
    }

    [HttpPatch("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePasswordUser model)
    {
        var user = await userService.GetByIdAsync(id);
        user.PasswordHash = PasswordHasher.HashPassword(model.Password);
        user = await userService.ReplaceOneAsync(user.Id, user);
        return Ok(user);
    }

    [HttpDelete("{id}"), PersonalAuthorize(UserRole = UserRole.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await userService.GetByIdAsync(id);
        await userService.DeleteOneAsync(user.Id);
        return NoContent();
    }
}
