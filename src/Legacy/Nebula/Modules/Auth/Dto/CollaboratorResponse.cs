using Nebula.Modules.Auth.Helpers;

namespace Nebula.Modules.Auth.Dto;

public class CollaboratorResponse
{
    public string CollaboratorId { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserRole { get; set; } = CompanyRoles.User;
}
