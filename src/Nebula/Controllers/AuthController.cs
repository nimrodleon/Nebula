using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nebula.Data.Helpers;
using Nebula.Data.Services;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public AuthController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("Login")]
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
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // Leemos el secretKey desde nuestro appSettings.json
                var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));
                var credentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature);
                var expires = DateTime.UtcNow.AddHours(12);
                var token = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: credentials);
                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token), expires});
            }
            catch (Exception)
            {
                ModelState.AddModelError("Login", "Usuario/Contraseña Invalida!");
                return BadRequest(ModelState);
            }
        }
    }
}
