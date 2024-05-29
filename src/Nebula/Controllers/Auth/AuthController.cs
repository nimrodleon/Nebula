using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;

namespace Nebula.Controllers.Auth;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController(
    IJwtService jwtService,
    IUserService userService,
    IUserAuthenticationService userAuthenticationService)
    : ControllerBase
{
    [HttpGet("UserData")]
    public Task<IActionResult> GetUserData()
    {
        try
        {
            var userAuth = userAuthenticationService.GetUserAuth();
            return Task.FromResult<IActionResult>(Ok(userAuth));
        }
        catch (Exception)
        {
            return Task.FromResult<IActionResult>(Unauthorized(new
                { ok = false, msg = "Error al recuperar los datos del usuario autenticado." }));
        }
    }

    [HttpPost("Login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthLogin model)
    {
        try
        {
            var user = await userService.GetByEmailAsync(model.Email);
            if (user is null) throw new Exception();

            if (PasswordHasher.VerifyHashedPassword(user.PasswordHash, model.Password)
                .Equals(PasswordVerificationResult.Failed)) throw new Exception();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("AccountType", user.AccountType),
                new Claim("UserRole", user.UserRole),
                new Claim("DefaultCompanyId", user.DefaultCompanyId),
            };

            var token = jwtService.GenerateToken(claims, 1000);
            return Ok(new { token });
        }
        catch (Exception)
        {
            ModelState.AddModelError("Login", "Usuario/Contraseña Invalida!");
            return BadRequest(ModelState);
        }
    }
}
