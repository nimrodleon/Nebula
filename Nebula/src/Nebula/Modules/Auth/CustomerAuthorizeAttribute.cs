using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nebula.Common;
using Nebula.Modules.Auth.Helpers;
using System.Security.Claims;

namespace Nebula.Modules.Auth;

public class CustomerAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public string UserRole { get; set; } = string.Empty;

    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        var _cacheAuthService = context.HttpContext.RequestServices
            .GetService(typeof(ICacheAuthService)) as ICacheAuthService;
        var companyId = context.RouteData.Values["companyId"]?.ToString();
        var user = context.HttpContext.User;

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && _cacheAuthService != null)
        {
            var userCompanyRoles = await _cacheAuthService.GetUserAuthCompanyRolesAsync(userIdClaim.Value);
            var userType = user.FindFirst("UserType")?.Value;
            if (userCompanyRoles != null && userType != null)
            {
                if (userType == UserTypeSystem.Customer)
                {
                    var roles = UserRole.Split(":");
                    // Verificar si el usuario tiene acceso a esta empresa con el rol adecuado
                    if (userCompanyRoles.Any(cr => cr.CompanyId == companyId && roles.Any(role => role == cr.UserRole))) return;
                }
            }
        }

        // El usuario no tiene acceso a esta empresa o no tiene el rol adecuado
        context.Result = new ForbidResult();
    }
}
