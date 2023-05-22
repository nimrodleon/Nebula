using Nebula.Common;
using Nebula.Modules.Configurations;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Sales.Dto;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales;

public class ConsultarValidezComprobanteService
{
    private readonly ConfigurationService _configurationService;
    private readonly CrudOperationService<InvoiceSerie> _invoiceSerieService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly CreditNoteService _creditNoteService;

    public ConsultarValidezComprobanteService(ConfigurationService configurationService,
        CrudOperationService<InvoiceSerie> invoiceSerieService, InvoiceSaleService invoiceSaleService,
        CreditNoteService creditNoteService)
    {
        _configurationService = configurationService;
        _invoiceSerieService = invoiceSerieService;
        _invoiceSaleService = invoiceSaleService;
        _creditNoteService = creditNoteService;
    }

    public async Task<string> CrearArchivosDeValidación(QueryConsultarValidezComprobante query)
    {
        List<InvoiceSale> invoiceSales = new List<InvoiceSale>();
        List<CreditNote> creditNotes = new List<CreditNote>();
        List<InvoiceSerie> invoiceSeries = await _invoiceSerieService.GetAsync("Name", string.Empty);
        // Obtener comprobantes por dia.
        if (query.Type.Equals(TypeConsultarValidez.Dia))
        {
            invoiceSales = await _invoiceSaleService.GetInvoiceSaleByDate(query.Date);
            creditNotes = await _creditNoteService.GetCreditNotesByDate(query.Date);
        }

        // Obtener comprobantes por mes.
        if (query.Type.Equals(TypeConsultarValidez.Mensual))
        {
            invoiceSales = await _invoiceSaleService.GetInvoiceSaleByMonthAndYear(query.Month, query.Year);
            creditNotes = await _creditNoteService.GetCreditNotesByMonthAndYear(query.Month, query.Year);
        }

        // generar archivos planos.
        var configuration = await _configurationService.GetAsync();
        var generarArchivo = new GenerarArchivoValidezComprobante(invoiceSeries, invoiceSales, creditNotes);
        return generarArchivo.CrearArchivosDeValidación(configuration.Ruc);
    }
}
