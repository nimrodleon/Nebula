using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Models;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth.Dto;
using Nebula.Common;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Nebula.Controllers.Auth;

[Route("api/auth/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public UserController(IUserService userService, IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRegister model)
    {
        var user = new User()
        {
            UserName = model.UserName,
            Email = model.Email,
            PasswordHash = PasswordHasher.HashPassword(model.Password),
            UserType = UserTypeSystem.Customer,
        };
        await _userService.CreateAsync(user);

        // enviar correo electrónico.
        string fromEmail = "reddrc21@gmail.com";
        string subject = $"Confirmación de Correo Electrónico - {user.Email.Trim()}";
        string body = $"""
        Hola {user.UserName},
        Gracias por registrarte en nuestro sitio web.<br>
        Para completar tu registro, por favor haz clic en el siguiente enlace para validar tu dirección de correo electrónico:
        <br>
        http://localhost:5042/swagger/index.html
        <br>
        Si no puedes hacer clic en el enlace, cópialo y pégalo en la barra de direcciones de tu navegador web.
        <br>
        ¡Gracias por unirte a nosotros!
        <br>
        Atentamente,
        CPEDIGITAL.NET
        """;
        await _emailService.SendEmailAsync(fromEmail, user.Email.Trim(), subject, body);
        return Ok(user);
    }

    //[HttpGet]
    //public async Task<IActionResult> Index([FromQuery] string query = "")
    //{
    //    var users = await _userService.GetListAsync(query);
    //    return Ok(users);
    //}

    //[HttpGet("{id}")]
    //public async Task<IActionResult> Show(string id)
    //{
    //    var user = await _userService.GetByIdAsync(id);
    //    return Ok(user);
    //}

    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(string id, [FromBody] UserRegister model)
    //{
    //    var user = await _userService.GetByIdAsync(id);
    //    user.UserName = model.UserName;
    //    user.Email = model.Email;
    //    await _userService.UpdateAsync(id, user);
    //    return Ok(user);
    //}

    //[HttpPut("PasswordChange/{id}")]
    //public async Task<IActionResult> PasswordChange(string id, [FromBody] UserRegister model)
    //{
    //    var user = await _userService.GetByIdAsync(id);
    //    user.PasswordHash = PasswordHasher.HashPassword(model.Password);
    //    await _userService.UpdateAsync(id, user);
    //    return Ok(user);
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(string id)
    //{
    //    var user = await _userService.GetByIdAsync(id);
    //    await _userService.RemoveAsync(id);
    //    return Ok(user);
    //}
}
