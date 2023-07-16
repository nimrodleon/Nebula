using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nebula.Modules.Auth.Models;
using System.Security.Claims;

namespace Nebula.Modules.Auth;

public class UserAuthorizeAttribute : TypeFilterAttribute
{
    public UserAuthorizeAttribute(params string[] roles) : base(typeof(UserAuthorizeFilter))
    {
        Arguments = new object[] { roles };
    }
}

public class UserAuthorizeFilter : IAsyncAuthorizationFilter
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly string[] _roles;

    public UserAuthorizeFilter(IUserService userService, IRoleService roleService, string[] roles)
    {
        _userService = userService;
        _roleService = roleService;
        _roles = roles;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Obtener el usuario actual autenticado
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.GetByIdAsync(userId);

        // Verificar si el usuario tiene los permisos requeridos para acceder al recurso
        if (!await CheckPermissionsAsync(context.HttpContext, user))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }
    }

    private async Task<bool> CheckPermissionsAsync(HttpContext context, User user)
    {
        // Obtener los permisos requeridos del atributo UserAuthorize en el controlador o acción
        var requiredRoles = _roles;

        // Verificar si el usuario tiene al menos uno de los permisos requeridos
        if (requiredRoles != null && requiredRoles.Length > 0 && user != null)
        {
            var role = await _roleService.GetByIdAsync(user.RolesId);
            if (role != null && requiredRoles.Intersect(role.Permisos).Any())
            {
                return true;
            }
        }

        return false;
    }
}
