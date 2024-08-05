using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Models;

namespace Nebula.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class UserBusinessController(
    IUserService userService,
    ICompanyService companyService
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRegisterBusiness model)
    {
        try
        {
            // crear usuario
            var user = new User()
            {
                UserName = model.UserName,
                PasswordHash = PasswordHasher.HashPassword(model.Password.Trim()),
                Email = model.Email,
                AccountType = AccountTypeHelper.Business,
                UserRole = UserRoleDbHelper.Admin,
                FullName = model.FullName.ToUpper(),
                PhoneNumber = model.PhoneNumber,
                LocalDefault = "-.-"
            };
            user = await userService.InsertOneAsync(user);
            // crear empresa
            var company = new Company()
            {
                Ruc = "00000000000",
                RznSocial = "DEMO S.A.C.",
                Address = "ANDAHUAYLAS - APURÍMAC - PERÚ",
                UserId = user.Id
            };
            await companyService.SingleInsertOneAsync(company);
            // actualizar usuario
            user.LocalDefault = company.Id;
            await userService.ReplaceOneAsync(user.Id, user);
            return Ok(user);
        }
        catch (MongoWriteException ex)
            when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un usuario con el mismo nombre de usuario.");
        }
    }
}
