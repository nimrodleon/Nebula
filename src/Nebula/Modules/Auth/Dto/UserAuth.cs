using Nebula.Modules.Auth.Helpers;

namespace Nebula.Modules.Auth.Dto;

public class UserAuth
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string AccountType { get; set; } = AccountTypeHelper.Personal;
    public string UserRole { get; set; } = UserRoleDbHelper.User;
    public string DefaultCompanyId { get; set; } = string.Empty;
}
