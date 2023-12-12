using Nebula.Modules.Taller.Services;

namespace Nebula.Modules.Taller;

public static class ModuleTaller
{
    public static IServiceCollection AddModuleTallerServices(this IServiceCollection services)
    {
        services.AddScoped<ITallerRepairOrderService, TallerRepairOrderService>();
        services.AddScoped<ITallerItemRepairOrderService, TallerItemRepairOrderService>();
        return services;
    }
}
