using Nebula.Database.Helpers;
using Nebula.Database.Models.Cashier;
using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;
using Nebula.Database.Services.Common;
using Nebula.Database.Services.Sales;
using Nebula.Database.Dto.Sales;

namespace Nebula.Database.Services.Cashier;

/// <summary>
/// Servicio para gestionar ventas.
/// </summary>
public class CashierSaleService
{
    private readonly ConfigurationService _configurationService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;
    private readonly CrudOperationService<InvoiceSerie> _invoiceSerieService;
    private readonly CashierDetailService _cashierDetailService;

    public CashierSaleService(ConfigurationService configurationService,
        InvoiceSaleService invoiceSaleService, InvoiceSaleDetailService invoiceSaleDetailService,
        TributoSaleService tributoSaleService, CrudOperationService<InvoiceSerie> invoiceSerieService, CashierDetailService cashierDetailService)
    {
        _configurationService = configurationService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _invoiceSerieService = invoiceSerieService;
        _cashierDetailService = cashierDetailService;
    }

    /// <summary>
    /// modelo de datos.
    /// </summary>
    private ComprobanteDto comprobanteDto = new ComprobanteDto();

    /// <summary>
    /// cargar modelo de datos al servicio.
    /// </summary>
    public void SetComprobanteDto(ComprobanteDto dto) { comprobanteDto = dto; }

    /// <summary>
    /// Guardar comprobante de venta rápida.
    /// </summary>
    public async Task<InvoiceSale> SaveChangesAsync(string cajaDiariaId)
    {
        var configuration = await _configurationService.GetAsync();
        comprobanteDto.SetConfiguration(configuration);

        var invoiceSale = comprobanteDto.GetInvoiceSale();
        var invoiceSerieId = comprobanteDto.Cabecera.InvoiceSerieId;
        var invoiceSerie = await _invoiceSerieService.GetAsync(invoiceSerieId);
        comprobanteDto.GenerarSerieComprobante(ref invoiceSerie, ref invoiceSale);

        // Agregar Información del comprobante.
        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        await _invoiceSaleService.CreateAsync(invoiceSale);

        // Agregar detalles del comprobante.
        var invoiceSaleDetails = comprobanteDto.GetInvoiceSaleDetails(invoiceSale.Id);
        await _invoiceSaleDetailService.CreateManyAsync(invoiceSaleDetails);

        // Agregar Tributos de Factura.
        var tributoSales = comprobanteDto.GetTributoSales(invoiceSale.Id);
        await _tributoSaleService.CreateManyAsync(tributoSales);

        // Registrar operación de Caja.
        var cashierDetail = new CashierDetail()
        {
            CajaDiaria = cajaDiariaId,
            InvoiceSale = invoiceSale.Id,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Number}",
            ContactId = invoiceSale.ContactId,
            ContactName = invoiceSale.RznSocialUsuario,
            Remark = comprobanteDto.Cabecera.Remark,
            TypeOperation = TypeOperationCaja.ComprobanteDeVenta,
            FormaPago = comprobanteDto.DatoPago.FormaPago,
            Amount = invoiceSale.SumImpVenta
        };
        await _cashierDetailService.CreateAsync(cashierDetail);
        return invoiceSale;
    }
}
