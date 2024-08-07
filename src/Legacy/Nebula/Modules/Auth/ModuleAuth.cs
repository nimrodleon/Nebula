namespace Nebula.Modules.Auth;

public static class ModuleAuth
{
    public static IServiceCollection AddModuleAuthServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICollaboratorService, CollaboratorService>();
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}
