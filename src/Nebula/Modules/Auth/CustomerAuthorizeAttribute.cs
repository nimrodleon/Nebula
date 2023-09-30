using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Helpers;
using System.Text.Json;

namespace Nebula.Modules.Auth;

public class CustomerAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public string UserRole { get; set; } = string.Empty;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var companyId = context.RouteData.Values["companyId"]?.ToString();
        var user = context.HttpContext.User;

        var companyUserRolesJson = user.FindFirst("CompanyUserRoles")?.Value;
        var userType = user.FindFirst("UserType")?.Value;
        if (companyUserRolesJson != null && userType != null)
        {
            if (userType == UserTypeSystem.Customer)
            {
                var roles = UserRole.Split(":");
                var companyUserRoles = JsonSerializer.Deserialize<List<CompanyUserRoleInfo>>(companyUserRolesJson);
                // Verificar si el usuario tiene acceso a esta empresa con el rol adecuado
                if (companyUserRoles != null && companyUserRoles.Any(cr =>
                    cr.CompanyId == companyId && roles.Any(role => role == cr.UserRole))) return;
            }
        }

        // El usuario no tiene acceso a esta empresa o no tiene el rol adecuado
        context.Result = new ForbidResult();
    }
}
