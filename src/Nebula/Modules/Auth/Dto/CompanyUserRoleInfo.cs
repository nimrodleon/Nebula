using Nebula.Modules.Auth.Helpers;

namespace Nebula.Modules.Auth.Dto;

public class CompanyUserRoleInfo
{
    public string CompanyId { get; set; } = string.Empty;
    public string UserRole { get; set; } = CompanyRoles.User;
}
