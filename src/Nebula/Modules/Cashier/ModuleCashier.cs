namespace Nebula.Modules.Cashier;

public static class ModuleCashier
{
    public static IServiceCollection AddModuleCashierServices(this IServiceCollection services)
    {
        services.AddScoped<ICajaDiariaService, CajaDiariaService>();
        services.AddScoped<ICashierDetailService, CashierDetailService>();
        services.AddScoped<ICashierSaleService, CashierSaleService>();

        return services;
    }
}
