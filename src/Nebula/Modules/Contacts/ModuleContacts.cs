namespace Nebula.Modules.Contacts;

public static class ModuleContacts
{
    public static IServiceCollection AddModuleContactsServices(this IServiceCollection services)
    {
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IContribuyenteService, ContribuyenteService>();
        return services;
    }
}
