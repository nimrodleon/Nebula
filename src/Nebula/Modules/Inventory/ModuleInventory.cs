using Nebula.Modules.Inventory.Stock;

namespace Nebula.Modules.Inventory;

public static class ModuleInventory
{
    public static IServiceCollection AddModuleInventoryServices(this IServiceCollection services)
    {
        services.AddScoped<IProductStockService, ProductStockService>();

        return services;
    }
}
