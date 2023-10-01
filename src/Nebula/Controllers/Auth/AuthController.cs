using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    private readonly IJwtService _jwtService;

    public AuthController(IJwtService jwtService,
        IUserService userService, ICollaboratorService collaboratorService)
    {
        _jwtService = jwtService;
        _userService = userService;
        _collaboratorService = collaboratorService;
    }

    [HttpPost("Login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthLogin model)
    {
        try
        {
            var user = await _userService.GetByUserNameAsync(model.UserName);
            if (user is null) throw new Exception();

            var collaborations = await _collaboratorService.GetCollaborationsByUserIdAsync(user.Id);
            var companyUserRoles = new List<CompanyUserRoleInfo>();
            collaborations.ForEach(item => companyUserRoles.Add(new CompanyUserRoleInfo()
            {
                CompanyId = item.CompanyId,
                UserRole = item.UserRole
            }));

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
