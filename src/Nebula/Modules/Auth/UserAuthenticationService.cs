using System.Security.Claims;

namespace Nebula.Modules.Auth;

public interface IUserAuthenticationService
{
    string GetUserId();
}

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAuthenticationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId ?? string.Empty;
    }
}
