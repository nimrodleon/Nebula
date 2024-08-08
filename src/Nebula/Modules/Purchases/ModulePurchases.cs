namespace Nebula.Modules.Purchases;

public static class ModulePurchases
{
    public static IServiceCollection AddModulePurchasesServices(this IServiceCollection services)
    {
        services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
        services.AddScoped<IPurchaseInvoiceDetailService, PurchaseInvoiceDetailService>();
        services.AddScoped<IConsultarValidezCompraService, ConsultarValidezCompraService>();
        return services;
    }
}
