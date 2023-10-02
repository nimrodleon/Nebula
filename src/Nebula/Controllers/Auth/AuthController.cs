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
    private readonly IUserAuthenticationService _userAuthenticationService;

    public AuthController(IJwtService jwtService, ICacheService cacheService,
        IUserService userService, ICollaboratorService collaboratorService,
        ICompanyService companyService, IUserAuthenticationService userAuthenticationService)
    {
        _jwtService = jwtService;
        _cacheService = cacheService;
        _userService = userService;
        _collaboratorService = collaboratorService;
        _companyService = companyService;
        _userAuthenticationService = userAuthenticationService;
    }

    [HttpGet("UserData")]
    public async Task<IActionResult> GetUserData()
    {
        try
        {
            var userId = _userAuthenticationService.GetUserId();
            var user = await _cacheService.GetUserAuthAsync(userId);
            if (user == null) throw new Exception();
            var companies = await _cacheService.GetUserAuthCompaniesAsync(userId);
            var companyRoles = await _cacheService.GetUserAuthCompanyRolesAsync(userId);

            var userAuthData = new UserAuth
            {
                User = user,
                Companies = companies,
                CompanyRoles = companyRoles
            };

            return Ok(userAuthData);
        }
        catch (Exception)
        {
            return BadRequest(new { ok = false, msg = "Error al recuperar los datos del usuario autenticado." });
        }
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
            var companyUserRoles = new List<UserCompanyRole>();
            List<string> companyIds = new List<string>();
            collaborations.ForEach(item =>
            {
                companyUserRoles.Add(new UserCompanyRole()
                {
                    CompanyId = item.CompanyId,
                    UserRole = item.UserRole
                });
                companyIds.Add(item.CompanyId);
            });

            var companies = await _companyService.GetCompaniesByIds(companyIds.ToArray());
            await _cacheService.SetUserAuthCompaniesAsync(user.Id, companies);
            await _cacheService.SetUserAuthCompanyRolesAsync(user.Id, companyUserRoles);

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

    [HttpDelete("Logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userId = _userAuthenticationService.GetUserId(); ; // Obtener el ID del usuario autenticado
            await _cacheService.RemoveUserAuthAsync(userId); // Borrar el registro de usuario en Redis
            await _cacheService.RemoveUserAuthCompaniesAsync(userId); // Borrar el registro de compañías en Redis
            await _cacheService.RemoveUserAuthCompanyRolesAsync(userId); // Borrar el registro de roles de la compañía en Redis

            return Ok(new { ok = true, msg = "Logout exitoso." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ok = false, msg = $"Error al realizar el logout: {ex.Message}" });
        }
    }
}
