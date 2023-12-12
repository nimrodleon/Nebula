namespace Nebula.Modules.InvoiceHub;

public static class ModuleInvoiceHub
{
    public static IServiceCollection AddModuleInvoiceHubServices(this IServiceCollection services)
    {
        services.AddHttpClient<ICreditNoteHubService, CreditNoteHubService>();
        services.AddHttpClient<IInvoiceHubService, InvoiceHubService>();
        services.AddHttpClient<ICertificadoUploaderService, CertificadoUploaderService>();
        services.AddHttpClient<IEmpresaHubService, EmpresaHubService>();
        return services;
    }
}
