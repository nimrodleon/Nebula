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

public class ConsultarValidezComprobanteService : IConsultarValidezComprobanteService
{
    private readonly IInvoiceSerieService _invoiceSerieService;
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly ICreditNoteService _creditNoteService;

    public ConsultarValidezComprobanteService(IInvoiceSerieService invoiceSerieService,
        IInvoiceSaleService invoiceSaleService, ICreditNoteService creditNoteService)
    {
        _invoiceSerieService = invoiceSerieService;
        _invoiceSaleService = invoiceSaleService;
        _creditNoteService = creditNoteService;
    }

    public async Task<string> CrearArchivosDeValidación(Company company, QueryConsultarValidezComprobante query)
    {
        List<InvoiceSale> invoiceSales = new List<InvoiceSale>();
        List<CreditNote> creditNotes = new List<CreditNote>();
        string[] fieldNames = new string[] { "Name" };
        List<InvoiceSerie> invoiceSeries = await _invoiceSerieService.GetFilteredAsync(company.Id, fieldNames, string.Empty);
        // Obtener comprobantes por dia.
        if (query.Type.Equals(TypeConsultarValidez.Dia))
        {
            invoiceSales = await _invoiceSaleService.GetInvoiceSaleByDate(company.Id, query.Date);
            creditNotes = await _creditNoteService.GetCreditNotesByDate(company.Id, query.Date);
        }

        // Obtener comprobantes por mes.
        if (query.Type.Equals(TypeConsultarValidez.Mensual))
        {
            invoiceSales = await _invoiceSaleService.GetInvoiceSaleByMonthAndYear(company.Id, query.Month, query.Year);
            creditNotes = await _creditNoteService.GetCreditNotesByMonthAndYear(company.Id, query.Month, query.Year);
        }

        // generar archivos planos.
        var generarArchivo = new GenerarArchivoValidezComprobante(invoiceSeries, invoiceSales, creditNotes);
        return generarArchivo.CrearArchivosDeValidación(company.Ruc);
    }
}
