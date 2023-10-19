using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;

namespace Nebula.Controllers.Auth;

[Authorize]
[Route("api/auth/[controller]")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorController(IUserService userService,
        ICollaboratorService collaboratorService)
    {
        _userService = userService;
        _collaboratorService = collaboratorService;
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

}
