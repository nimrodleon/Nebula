using Nebula.Modules.Auth.Helpers;

namespace Nebula.Modules.Auth.Dto;

public class InviteCollaborator
{
    public string CompanyId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserRole { get; set; } = CompanyRoles.User;
}
