using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpGet("GetMe"), UserAuthorize(Permission.ConfigurationRead)]
    public Task<IActionResult> GetMe()
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        var role = User.FindFirstValue(ClaimTypes.Role);
        return Task.FromResult<IActionResult>(Ok(new { userName, role }));
    }

    [HttpGet("CreateSupportUser"), AllowAnonymous]
    public IActionResult CreateSupportUser()
    {
        var result = _configuration.GetValue<bool>("CreateSupportUser");
        return Ok(result);
    }

    [HttpPost("Login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthLogin model)
    {
        try
        {
            var user = await _userService.GetByUserNameAsync(model.UserName);
            if (user is null) throw new Exception();

            if (PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password)
                .Equals(PasswordVerificationResult.Failed)) throw new Exception();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.RolesId)
            };

            // Leemos el secretKey desde nuestro appSettings.json
            var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));
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
