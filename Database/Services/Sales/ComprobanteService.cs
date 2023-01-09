using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;
using Nebula.Database.Services.Common;
using Nebula.Database.Dto.Sales;
using Nebula.Database.Helpers;

namespace Nebula.Database.Services.Sales;

public class ComprobanteService
{
    private readonly ConfigurationService _configurationService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;
    private readonly CrudOperationService<InvoiceSerie> _invoiceSerieService;
    private readonly DetallePagoSaleService _detallePagoSaleService;

    public ComprobanteService(ConfigurationService configurationService,
        InvoiceSaleService invoiceSaleService, InvoiceSaleDetailService invoiceSaleDetailService,
        TributoSaleService tributoSaleService, CrudOperationService<InvoiceSerie> invoiceSerieService,
        DetallePagoSaleService detallePagoSaleService)
    {
        _configurationService = configurationService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _invoiceSerieService = invoiceSerieService;
        _detallePagoSaleService = detallePagoSaleService;
    }

    /// <summary>
    /// modelo de datos.
    /// </summary>
    private ComprobanteDto comprobanteDto = new ComprobanteDto();

    /// <summary>
    /// cargar modelo de datos al servicio.
    /// </summary>
    public void SetComprobanteDto(ComprobanteDto dto) { comprobanteDto = dto; }

    public async Task<InvoiceSale> SaveChangesAsync()
    {
        var configuration = await _configurationService.GetAsync();
        comprobanteDto.SetConfiguration(configuration);
        var invoiceSale = comprobanteDto.GetInvoiceSale();
        var invoiceSerieId = comprobanteDto.Cabecera.InvoiceSerieId;
        var invoiceSerie = await _invoiceSerieService.GetAsync(invoiceSerieId);
        comprobanteDto.GenerarSerieComprobante(ref invoiceSerie, ref invoiceSale);
        invoiceSale.InvoiceSerieId = invoiceSerie.Id;

        // agregar Información del comprobante.
        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        await _invoiceSaleService.CreateAsync(invoiceSale);

        // agregar detalles del comprobante.
        var invoiceSaleDetails = comprobanteDto.GetInvoiceSaleDetails(invoiceSale.Id);
        await _invoiceSaleDetailService.CreateManyAsync(invoiceSaleDetails);

        // agregar Tributos de Factura.
        var tributoSales = comprobanteDto.GetTributoSales(invoiceSale.Id);
        await _tributoSaleService.CreateManyAsync(tributoSales);

        // registrar detalle de pago si la operación es a crédito.
        if (comprobanteDto.DatoPago.FormaPago == FormaPago.Credito)
        {
            if (invoiceSale.DocType == "FACTURA")
            {
                var detallePagos = comprobanteDto.GetDetallePagos(invoiceSale.Id);
                if (detallePagos.Count() > 0) await _detallePagoSaleService.InsertManyAsync(detallePagos);
            }
        }

        return invoiceSale;
    }
}
