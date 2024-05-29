namespace Nebula.Modules.Account;

public static class ModuleAccount
{
    public static IServiceCollection AddModuleAccountServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IInvoiceSerieService, InvoiceSerieService>();
        return services;
    }
}
