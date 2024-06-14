using Nebula.Modules.Auth.Helpers;

namespace Nebula.Modules.Auth.Dto;

public class UserRegisterPersonal
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserRole { get; set; } = UserRoleDbHelper.User;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
