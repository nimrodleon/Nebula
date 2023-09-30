using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;

namespace Nebula.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly ICollaboratorService _collaboratorService;

    public AuthController(IConfiguration configuration,
        IUserService userService, ICollaboratorService collaboratorService)
    {
        _configuration = configuration;
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
            collaborations.ForEach(item => companyUserRoles.Add(new CompanyUserRoleInfo() {
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

            // Leemos el secretKey desde nuestro appSettings.json
            string defaultKey = "3b6ef85c442b797567051e90df5a85ec6e22675203de344787718af0f140fc54";
            var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey") ?? defaultKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddHours(12);
            var token = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: credentials);
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expires });
        }
        catch (Exception)
        {
            ModelState.AddModelError("Login", "Usuario/Contraseña Invalida!");
            return BadRequest(ModelState);
        }
    }
}
