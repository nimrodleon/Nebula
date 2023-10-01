using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common;
using Nebula.Modules.Account;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;

namespace Nebula.Controllers.Auth;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICollaboratorService _collaboratorService;
    private readonly ICompanyService _companyService;
    private readonly IJwtService _jwtService;
    private readonly ICacheService _cacheService;

    public AuthController(IJwtService jwtService, ICacheService cacheService,
        IUserService userService, ICollaboratorService collaboratorService,
        ICompanyService companyService)
    {
        _jwtService = jwtService;
        _cacheService = cacheService;
        _userService = userService;
        _collaboratorService = collaboratorService;
        _companyService = companyService;
    }

    [HttpPost("Login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthLogin model)
    {
        try
        {
            var user = await _userService.GetByEmailAsync(model.Email);
            if (user is null) throw new Exception();
            await _cacheService.SetUserAuthAsync(user);

            var collaborations = await _collaboratorService.GetCollaborationsByUserIdAsync(user.Id);
            var companyUserRoles = new List<CompanyUserRoleInfo>();
            List<string> companyIds = new List<string>();
            collaborations.ForEach(item =>
            {
                companyUserRoles.Add(new CompanyUserRoleInfo()
                {
                    CompanyId = item.CompanyId,
                    UserRole = item.UserRole
                });
                companyIds.Add(item.CompanyId);
            });

            var companies = await _companyService.GetCompaniesByIds(companyIds.ToArray());
            await _cacheService.SetUserAuthCompaniesAsync(user.Id, companies);

            if (PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password)
                .Equals(PasswordVerificationResult.Failed)) throw new Exception();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserType", user.UserType),
                new Claim("CompanyUserRoles", JsonSerializer.Serialize(companyUserRoles)),
            };

            var token = _jwtService.GenerateToken(claims, 1000);
            return Ok(new { token });
        }
        catch (Exception)
        {
            ModelState.AddModelError("Login", "Usuario/Contraseña Invalida!");
            return BadRequest(ModelState);
        }
    }
}
