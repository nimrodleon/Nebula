using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Helpers;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Models;
using Nebula.Modules.Sales.Notes;

namespace Nebula.Modules.Sales;

public interface IConsultarValidezComprobanteService
{
    Task<string> CrearArchivosDeValidación(Company company, QueryConsultarValidezComprobante query);
}

public class ConsultarValidezComprobanteService(
    IInvoiceSerieService invoiceSerieService,
    IInvoiceSaleService invoiceSaleService,
    ICreditNoteService creditNoteService)
    : IConsultarValidezComprobanteService
{
    public async Task<string> CrearArchivosDeValidación(Company company, QueryConsultarValidezComprobante query)
    {
        List<InvoiceSale> invoiceSales = new List<InvoiceSale>();
        List<CreditNote> creditNotes = new List<CreditNote>();
        string[] fieldNames = new string[] { "Name" };
        List<InvoiceSerie> invoiceSeries =
            await invoiceSerieService.GetFilteredAsync(company.Id, fieldNames, string.Empty);
        // Obtener comprobantes por dia.
        if (query.Type.Equals(TypeConsultarValidez.Dia))
        {
            invoiceSales = await invoiceSaleService.GetInvoiceSaleByDate(company.Id, query.Date);
            creditNotes = await creditNoteService.GetCreditNotesByDate(company.Id, query.Date);
        }

        // Obtener comprobantes por mes.
        if (query.Type.Equals(TypeConsultarValidez.Mensual))
        {
            invoiceSales = await invoiceSaleService.GetInvoiceSaleByMonthAndYear(company.Id, query.Month, query.Year);
            creditNotes = await creditNoteService.GetCreditNotesByMonthAndYear(company.Id, query.Month, query.Year);
        }

        // generar archivos planos.
        var generarArchivo = new GenerarArchivoValidezComprobante(invoiceSeries, invoiceSales, creditNotes);
        return generarArchivo.CrearArchivosDeValidación(company.Ruc);
    }
}
