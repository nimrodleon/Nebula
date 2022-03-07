using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nebula.Data;
using Nebula.Data.Helpers;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRavenDbContext _context;

        public AuthController(IConfiguration configuration, IRavenDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthLogin model)
        {
            try
            {
                using var session = _context.Store.OpenAsyncSession();
                var user = await session.Query<User>().Where(m => m.UserName == model.UserName).SingleAsync();

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
