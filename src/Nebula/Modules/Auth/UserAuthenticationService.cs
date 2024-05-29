using System.Security.Claims;
using Nebula.Modules.Auth.Dto;

namespace Nebula.Modules.Auth;

public interface IUserAuthenticationService
{
    string GetUserId();
    string GetUserName();
    string GetAccountType();
    string GetUserRole();
    string GetDefaultCompanyId();
    UserAuth GetUserAuth();
}

public class UserAuthenticationService(IHttpContextAccessor httpContextAccessor) : IUserAuthenticationService
{
    public UserAuth GetUserAuth()
    {
        return new UserAuth
        {
            UserId = GetUserId(),
            UserName = GetUserName(),
            AccountType = GetAccountType(),
            UserRole = GetUserRole(),
            DefaultCompanyId = GetDefaultCompanyId()
        };
    }

    private string GetClaimValue(string claimType)
    {
        var claimValue = httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
        return claimValue ?? string.Empty;
    }

    public string GetUserId() => GetClaimValue(ClaimTypes.NameIdentifier);
    public string GetUserName() => GetClaimValue(ClaimTypes.Name);
    public string GetAccountType() => GetClaimValue("AccountType");
    public string GetUserRole() => GetClaimValue("UserRole");
    public string GetDefaultCompanyId() => GetClaimValue("DefaultCompanyId");
}
