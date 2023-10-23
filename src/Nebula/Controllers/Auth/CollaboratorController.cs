using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Nebula.Controllers.Auth;

[Authorize]
[Route("api/auth/[controller]")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly IDatabase _redis;
    private readonly ICacheAuthService _cacheAuthService;
    private readonly IUserService _userService;
    private readonly ICollaboratorService _collaboratorService;
    private readonly IEmailService _emailService;

    public CollaboratorController(IDatabase redis,
        ICacheAuthService cacheAuthService, IUserService userService,
        ICollaboratorService collaboratorService, IEmailService emailService)
    {
        _redis = redis;
        _cacheAuthService = cacheAuthService;
        _userService = userService;
        _collaboratorService = collaboratorService;
        _emailService = emailService;

    }

    [HttpGet("{companyId}")]
    public async Task<IActionResult> Index(string companyId)
    {
        var collaborators = await _collaboratorService.GetCollaborationsByCompanyId(companyId);
        List<string> userIds = new List<string>();
        collaborators.ForEach(item => userIds.Add(item.UserId));
        var users = await _userService.GetUsersByUserIds(userIds);
        var responses = new List<CollaboratorResponse>();
        collaborators.ForEach(item =>
        {
            responses.Add(new CollaboratorResponse
            {
                CollaboratorId = item.Id,
                CompanyId = item.CompanyId,
                UserId = item.UserId,
                UserName = users.Single(x => x.Id == item.UserId).UserName,
                Email = users.Single(x => x.Id == item.UserId).Email,
                UserRole = item.UserRole,
            });
        });
        return Ok(responses);
    }

    [HttpPost("Invite")]
    public async Task<IActionResult> Invite([FromBody] InviteCollaborator model)
    {
        string uuid = Guid.NewGuid().ToString();
        var jsonData = JsonSerializer.Serialize(model);
        await _redis.StringSetAsync($"nebula_invitar_colaborador_{uuid}", jsonData);
        await _redis.KeyExpireAsync($"nebula_invitar_colaborador_{uuid}", TimeSpan.FromMinutes(30));
        var company = await _cacheAuthService.GetCompanyByIdAsync(model.CompanyId.Trim());
        // enviar correo electrónico.
        string fromEmail = "reddrc21@gmail.com";
        string subject = $"Invitación para unirse a, {company.Ruc} - {company.RznSocial}";
        string body = $"""
            Hola {model.Email},
            Has sido invitado a unirte a {company.RznSocial} en nuestra plataforma.<br>
            Para configurar tu cuenta y empezar a gestionar la empresa, por favor haz clic en el siguiente enlace:
            <br>
            <a href='http://localhost:4200/account/companies/{company.Id}/collaborators/validate?uuid={uuid}'>Unirme a {company.RznSocial}</a>
            <br>
            Si no puedes hacer clic en el enlace, por favor cópialo y pégalo en la barra de direcciones de tu navegador web.
            <br>
            ¡Bienvenido a bordo!
            <br>
            Atentamente,
            El equipo de {company.RznSocial}
            """;
        await _emailService.SendEmailAsync(fromEmail, model.Email.Trim(), subject, body);
        return Ok(new { ok = true, msg = "El correo ha sido enviado." });
    }

    [HttpPost("Validate/{uuid}")]
    public async Task<IActionResult> Validate(string uuid)
    {
        var serializedData = await _redis.StringGetAsync($"nebula_invitar_colaborador_{uuid.Trim()}");
        if (serializedData.HasValue && !serializedData.IsNull)
        {
            var inviteCollaborator = JsonSerializer.Deserialize<InviteCollaborator>(serializedData,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var user = await _userService.GetByEmailAsync(inviteCollaborator.Email.Trim());
            if (user == null)
            {
                return BadRequest(new
                {
                    ok = false,
                    msg = "El usuario no se encuentra registrado. Por favor, regístrese para continuar.",
                    page = "REGISTER"
                });
            }

            try
            {
                var collaborator = new Collaborator()
                {
                    CompanyId = inviteCollaborator.CompanyId.Trim(),
                    UserId = user.Id,
                    UserRole = inviteCollaborator.UserRole.Trim()
                };
                await _collaboratorService.CreateAsync(collaborator);

                var userCompanyRole = new UserCompanyRole()
                {
                    CompanyId = inviteCollaborator.CompanyId.Trim(),
                    UserRole = inviteCollaborator.UserRole.Trim(),
                };

                var userAuth = await _cacheAuthService.GetUserAuthAsync(user.Id);
                if (userAuth == null)
                {
                    await _redis.KeyDeleteAsync($"nebula_invitar_colaborador_{uuid.Trim()}");
                    return Ok(new
                    {
                        ok = true,
                        msg = "¡Validación exitosa! Por favor, inicie sesión.",
                        page = "LOGIN"
                    });
                }
                else
                {
                    var userCompanyRoles = await _cacheAuthService.GetUserAuthCompanyRolesAsync(user.Id);
                    userCompanyRoles.Add(userCompanyRole);
                    await _cacheAuthService.RemoveUserAuthCompanyRolesAsync(user.Id);
                    await _cacheAuthService.SetUserAuthCompanyRolesAsync(user.Id, userCompanyRoles);
                    await _redis.KeyDeleteAsync($"nebula_invitar_colaborador_{uuid.Trim()}");
                    return Ok(new
                    {
                        ok = true,
                        msg = "Validación exitosa!, Su información ha sido verificada correctamente.",
                        page = "ACCOUNT"
                    });
                }
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return BadRequest(new
                {
                    ok = false,
                    msg = "El usuario ya se encuentra registrado.",
                    page = "ACCOUNT"
                });
            }
        }
        return BadRequest(new
        {
            ok = false,
            msg = "Lo sentimos, ha habido un error de validación.",
            page = "LOGIN"
        });
    }


}
