using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Models;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Dto;
using Nebula.Common;
using System.Security.Claims;
using System.Data.Common;
using MongoDB.Driver;
using Nebula.Common.Helpers;
using Nebula.Modules.Account;

namespace Nebula.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class UserController(
    IJwtService jwtService,
    IUserService userService,
    IEmailService emailService,
    ICompanyService companyService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var usuarios = await userService.GetListAsync(query, page, pageSize);
        var totalUsuarios = await userService.GetTotalListAsync(query);
        var totalPages = (int)Math.Ceiling((double)totalUsuarios / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<User>
        {
            Pagination = paginationInfo,
            Data = usuarios
        };

        return Ok(result);
    }

    [HttpGet("{userId}/Show")]
    public async Task<IActionResult> Show(string userId)
    {
        var user = await userService.GetByIdAsync(userId);
        return Ok(user);
    }

    [HttpGet("{userId}/Companies")]
    public async Task<IActionResult> Companies(string userId)
    {
        var companies = await companyService.GetCompaniesByUserIdAsync(userId);
        return Ok(companies);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegister model)
    {
        try
        {
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = PasswordHasher.HashPassword(model.Password),
                // UserType = UserTypeSystem.Customer,
                // IsEmailVerified = false,
            };
            user = await userService.InsertOneAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            string token = jwtService.GenerateToken(claims);
            // user.EmailValidationToken = token;
            await userService.ReplaceOneAsync(user.Id, user);

            // enviar correo electrónico.
            string fromEmail = "cpedigital12@gmail.com";
            string subject = $"Confirmación de Correo Electrónico - {user.Email.Trim()}";
            string body = $"""
                Hola {user.UserName},
                Gracias por registrarte en nuestro sitio web.<br>
                Para completar tu registro, por favor haz clic en el siguiente enlace para validar tu dirección de correo electrónico:
                <br>
                <a href='https://cloud.cpedigital.net/public/user/verify-email?token={token}'>Verificar E-Mail</a>
                <br>
                Si no puedes hacer clic en el enlace, cópialo y pégalo en la barra de direcciones de tu navegador web.
                <br>
                ¡Gracias por unirte a nosotros!
                <br>
                Atentamente,
                CPEDIGITAL.NET
                """;
            await emailService.SendEmailAsync(fromEmail, user.Email.Trim(), subject, body);
            return Ok(new { ok = true, msg = "Su registro ha sido exitoso. Por favor, revise su correo electrónico para confirmar su dirección de correo." });
        }
        catch (MongoWriteException ex)
        when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un usuario con el mismo email.");
        }
    }

    [HttpPost("RegisterFromAdmin")]
    public async Task<IActionResult> RegisterFromAdmin([FromBody] UserRegisterFromAdmin model)
    {
        try
        {
            var password = Guid.NewGuid().ToString();
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = PasswordHasher.HashPassword(password),
                // UserType = model.UserType,
                // IsEmailVerified = false,
                // Disabled = model.Disabled,
            };
            user = await userService.InsertOneAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            string token = jwtService.GenerateToken(claims);
            // user.EmailValidationToken = token;
            await userService.ReplaceOneAsync(user.Id, user);

            // enviar correo electrónico.
            string fromEmail = "cpedigital12@gmail.com";
            string subject = $"Confirmación de Correo Electrónico - {user.Email.Trim()}";
            string body = $"""
                Hola {user.UserName},
                Gracias por registrarte en nuestro sitio web.<br>
                Para completar tu registro, por favor haz clic en el siguiente enlace para validar tu dirección de correo electrónico:
                <br>
                <a href='https://cloud.cpedigital.net/public/user/verify-email?token={token}'>Verificar E-Mail</a>
                <br>
                user: {user.Email}
                <br>
                password: {password}
                <br>
                Si no puedes hacer clic en el enlace, cópialo y pégalo en la barra de direcciones de tu navegador web.
                <br>
                ¡Gracias por unirte a nosotros!
                <br>
                Atentamente,
                CPEDIGITAL.NET
                """;
            await emailService.SendEmailAsync(fromEmail, user.Email.Trim(), subject, body);
            return Ok(user);
        }
        catch (MongoWriteException ex)
        when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un usuario con el mismo email.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFromAdmin(string id, [FromBody] UserRegisterFromAdmin model)
    {
        try
        {
            var user = await userService.GetByIdAsync(id);
            if (user == null) return BadRequest();

            user.UserName = model.UserName;
            if (user.Email != model.Email) user.Email = model.Email;
            // user.UserType = model.UserType;
            // user.Disabled = model.Disabled;

            await userService.ReplaceOneAsync(user.Id, user);
            return Ok(user);
        }
        catch (MongoWriteException ex)
        when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un usuario con el mismo email.");
        }
    }

    [HttpGet("VerifyEmail")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        try
        {
            // Validar el token usando el servicio JWT
            var claimsPrincipal = jwtService.ValidateToken(token);
            if (claimsPrincipal == null)
            {
                return BadRequest(new
                {
                    ok = false,
                    msg = "Token inválido."
                });
            }

            // Obtener el ID del usuario desde el token
            string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var user = await userService.GetByIdAsync(userId);
            if (user == null)
                return BadRequest(new
                {
                    ok = false,
                    msg = "Usuario no encontrado."
                });
            // if (user.EmailValidationToken != token)
            //     return BadRequest(new
            //     {
            //         ok = false,
            //         msg = "Token no coincide con el usuario."
            //     });

            // Marcar el correo electrónico como verificado y limpiar el token
            // user.IsEmailVerified = true;
            // user.EmailValidationToken = string.Empty;

            // Actualizar el usuario en la base de datos
            await userService.ReplaceOneAsync(userId, user);

            return Ok(new
            {
                ok = true,
                msg = "Correo electrónico verificado correctamente."
            });
        }
        catch (DbException ex)
        {
            // Manejar excepciones específicas de la base de datos
            return BadRequest(new
            {
                ok = false,
                msg = $"Error al verificar el correo electrónico: {ex.Message}"
            });
        }
        catch (Exception ex)
        {
            // Manejar otras excepciones
            return BadRequest(new
            {
                ok = false,
                msg = $"Error al verificar el correo electrónico: {ex.Message}"
            });
        }
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
    {
        try
        {
            // Verificar si el correo electrónico está asociado a una cuenta de usuario
            var user = await userService.GetByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { ok = false, msg = "Correo electrónico no registrado." });
            }

            // Generar un token de restablecimiento de contraseña
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            string resetToken = jwtService.GenerateToken(claims);
            // user.ResetPasswordToken = resetToken;
            await userService.ReplaceOneAsync(user.Id, user);

            // Enviar correo electrónico con el enlace para restablecer la contraseña
            string resetPasswordLink = $"http://localhost:5042/api/auth/User/ResetPassword?token={resetToken}";
            string emailSubject = "Restablecimiento de Contraseña";
            string emailBody = $"Haz clic en el siguiente enlace para restablecer tu contraseña: {resetPasswordLink}";

            await emailService.SendEmailAsync("reddrc21@gmail.com", user.Email, emailSubject, emailBody);

            return Ok(new { ok = true, msg = "Correo electrónico de restablecimiento de contraseña enviado." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ok = false, msg = $"Error al procesar la solicitud: {ex.Message}" });
        }
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
    {
        try
        {
            // Validar el token usando el servicio JWT
            var claimsPrincipal = jwtService.ValidateToken(model.Token.Trim());
            if (claimsPrincipal == null) return BadRequest(new { ok = false, msg = "Token inválido." });

            // Obtener el ID del usuario desde el token
            string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var user = await userService.GetByIdAsync(userId);
            if (user == null) return BadRequest(new { ok = false, msg = "Usuario no encontrado." });
            // if (user.ResetPasswordToken != model.Token.Trim()) return BadRequest(new { ok = false, msg = "Token no coincide con el usuario." });

            // Actualizar la contraseña del usuario
            user.PasswordHash = PasswordHasher.HashPassword(model.NewPassword);

            // Limpiar el token de restablecimiento de contraseña
            // user.ResetPasswordToken = string.Empty;

            // Guardar la contraseña actualizada y limpiar el token en la base de datos
            await userService.ReplaceOneAsync(user.Id, user);

            return Ok(new { ok = true, msg = "Contraseña restablecida correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ok = false, msg = $"Error al restablecer la contraseña: {ex.Message}" });
        }
    }

}
