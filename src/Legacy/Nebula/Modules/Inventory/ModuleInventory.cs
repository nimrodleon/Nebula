using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Notas;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Transferencias;

namespace Nebula.Modules.Inventory;

public static class ModuleInventory
{
    public static IServiceCollection AddModuleInventoryServices(this IServiceCollection services)
    {
        services.AddScoped<IProductStockService, ProductStockService>();
        services.AddScoped<IHelperCalculateProductStockService, HelperCalculateProductStockService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<ILocationDetailService, LocationDetailService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddScoped<IMaterialDetailService, MaterialDetailService>();
        services.AddScoped<IInventoryNotasService, InventoryNotasService>();
        services.AddScoped<IInventoryNotasDetailService, InventoryNotasDetailService>();
        services.AddScoped<ITransferenciaService, TransferenciaService>();
        services.AddScoped<ITransferenciaDetailService, TransferenciaDetailService>();
        services.AddScoped<IAjusteInventarioService, AjusteInventarioService>();
        services.AddScoped<IAjusteInventarioDetailService, AjusteInventarioDetailService>();
        services.AddScoped<IValidateStockService, ValidateStockService>();

        return services;
    }
}
