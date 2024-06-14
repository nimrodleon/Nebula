using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Nebula.Modules.Auth.Helpers;

namespace Nebula.Modules.Auth;

public class BusinessAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var accountTypeClaim = user.FindFirstValue("AccountType");
        var rolesClaim = user.FindFirstValue("UserRole");
        var companyIdClaim = user.FindFirstValue("DefaultCompanyId");

        if (userIdClaim != null
            && accountTypeClaim != null
            && rolesClaim != null
            && companyIdClaim != null)
        {
            var roles = rolesClaim.Split(":");
            if (accountTypeClaim == AccountTypeHelper.Business
                && roles.Any(role => role == UserRole.Admin)) return;
        }

        // El usuario no tiene acceso a esta empresa o no tiene el rol adecuado
        context.Result = new ForbidResult();
    }
}
