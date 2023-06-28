using Nebula.Modules.Purchases.Helpers;
using Nebula.Modules.Purchases.Models;
using Nebula.Modules.Sales;

namespace Nebula.Modules.Purchases;

public interface IConsultarValidezCompraService
{
    Task<string> CrearArchivosDeValidacion(QueryConsultarValidezComprobante query);
}

public class ConsultarValidezCompraService : IConsultarValidezCompraService
{
    private readonly IPurchaseInvoiceService _purchaseInvoiceService;

    public ConsultarValidezCompraService(IPurchaseInvoiceService purchaseInvoiceService)
    {
        _purchaseInvoiceService = purchaseInvoiceService;
    }

    public async Task<string> CrearArchivosDeValidacion(QueryConsultarValidezComprobante query)
    {
        List<PurchaseInvoice> purchases = new List<PurchaseInvoice>();
        if (query.Type.Equals(TypeConsultarValidez.Dia))
            purchases = await _purchaseInvoiceService.GetByFecEmisionAsync(query.Date);
        if (query.Type.Equals(TypeConsultarValidez.Mensual))
            purchases = await _purchaseInvoiceService.GetByMonthAndYearAsync(query.Month, query.Year);
        var generarArchivo = new GenerarArchivoValidezCompra(purchases);
        return generarArchivo.CrearArchivosDeValidacion();
    }
}
