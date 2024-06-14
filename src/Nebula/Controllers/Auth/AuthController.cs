using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Account;
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
    ICompanyService companyService,
    IUserAuthenticationService userAuthenticationService)
    : ControllerBase
{
    [HttpGet("UserData")]
    public async Task<IActionResult> GetUserData()
    {
        try
        {
            var userAuth = userAuthenticationService.GetUserAuth();
            var userAuthConfig = new UserAuthConfig();
            var company = await companyService.GetByIdAsync(userAuth.DefaultCompanyId);
            userAuthConfig.UserAuth = userAuth;
            userAuthConfig.CompanyName = company.RznSocial;
            userAuthConfig.IsEnableModComprobante = company.ModComprobantes;
            userAuthConfig.IsEnableModCuentaPorCobrar = company.ModCuentaPorCobrar;
            userAuthConfig.IsEnableModReparaciones = company.ModReparaciones;
            userAuthConfig.IsEnableModCaja = company.ModCajasDiaria;
            return Ok(userAuthConfig);
        }
        catch (Exception)
        {
            return await Task.FromResult<IActionResult>(Unauthorized(new
                { ok = false, msg = "Error al recuperar los datos del usuario autenticado." }));
        }
    }

    [HttpPost("Login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthLogin model)
    {
        try
        {
            var user = await userService.GetByUserNameAsync(model.UserName.Trim());
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
