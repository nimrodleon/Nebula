using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nebula.Data.ViewModels;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthLogin model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Login", "Usuario/Contraseña Invalida!");
                return BadRequest(ModelState);
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Leemos el secretKey desde nuestro appsettings.json
            var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));
            var credentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddHours(12);
            var token = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: credentials);
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expires });
        }
    }
}
