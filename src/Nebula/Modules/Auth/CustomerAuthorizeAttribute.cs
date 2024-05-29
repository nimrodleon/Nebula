using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Nebula.Modules.Auth;

public class CustomerAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public string UserRole { get; set; } = string.Empty;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var rolesClaim = user.FindFirstValue("UserRole");
        var companyIdClaim = user.FindFirstValue("DefaultCompanyId");

        if (userIdClaim != null && rolesClaim != null && companyIdClaim != null)
        {
            var roles = rolesClaim.Split(":");
            if (roles.Any(role => role == UserRole)) return;
        }

        // El usuario no tiene acceso a esta empresa o no tiene el rol adecuado
        context.Result = new ForbidResult();
    }
}
