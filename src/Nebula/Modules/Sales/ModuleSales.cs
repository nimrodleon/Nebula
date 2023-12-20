using Nebula.Modules.Sales.Comprobantes;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Notes;

namespace Nebula.Modules.Sales;

public static class ModuleSales
{
    public static IServiceCollection AddModuleSalesServices(this IServiceCollection services)
    {
        services.AddScoped<IInvoiceSaleService, InvoiceSaleService>();
        services.AddScoped<IInvoiceSaleDetailService, InvoiceSaleDetailService>();
        services.AddScoped<IComprobanteService, ComprobanteService>();
        services.AddScoped<ICreditNoteService, CreditNoteService>();
        services.AddScoped<ICreditNoteDetailService, CreditNoteDetailService>();
        services.AddScoped<IConsultarValidezComprobanteService, ConsultarValidezComprobanteService>();
        services.AddScoped<IInvoiceSaleFileService, InvoiceSaleFileService>();

        return services;
    }
}
