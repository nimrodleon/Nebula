using Nebula.Data.Models.Cashier;
using Nebula.Data.Models.Common;
using Nebula.Data.Models.Sales;
using Nebula.Data.Services.Common;
using Nebula.Data.Services.Sales;
using Nebula.Data.ViewModels.Cashier;

namespace Nebula.Data.Services.Cashier;

/// <summary>
/// Servicio para gestionar ventas.
/// </summary>
public class CashierSaleService
{
    private readonly ConfigurationService _configurationService;
    private readonly ContactService _contactService;
    private readonly CajaDiariaService _cajaDiariaService;
    private readonly InvoiceSerieService _invoiceSerieService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;
    private readonly CashierDetailService _cashierDetailService;
    private Venta _venta;

    public CashierSaleService(ConfigurationService configurationService,
        ContactService contactService, CajaDiariaService cajaDiariaService,
        InvoiceSerieService invoiceSerieService, InvoiceSaleService invoiceSaleService,
        InvoiceSaleDetailService invoiceSaleDetailService, TributoSaleService tributoSaleService,
        CashierDetailService cashierDetailService)
    {
        _configurationService = configurationService;
        _contactService = contactService;
        _cajaDiariaService = cajaDiariaService;
        _invoiceSerieService = invoiceSerieService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _cashierDetailService = cashierDetailService;
        _venta = new Venta();
    }

    /// <summary>
    /// Establecer modelo venta.
    /// </summary>
    public void SetModel(Venta model) => _venta = model;

    /// <summary>
    /// Guardar comprobante de venta rápida.
    /// </summary>
    /// <param name="cajaDiariaId">ID caja diaria.</param>
    public async Task<InvoiceSale> CreateQuickSale(string cajaDiariaId)
    {
        var configuration = await _configurationService.GetAsync();
        var contact = await _contactService.GetAsync(_venta.ContactId);

        var invoiceSale = _venta.GetInvoice(configuration, contact);
        var cajaDiaria = await _cajaDiariaService.GetAsync(cajaDiariaId);
        var invoiceSerie = await _invoiceSerieService.GetAsync(cajaDiaria.Terminal.Split(":")[0].Trim());
        GenerateInvoiceSerie(ref invoiceSerie, ref invoiceSale, _venta.DocType);

        // Agregar Información del comprobante.
        await _invoiceSerieService.UpdateAsync(invoiceSale.Id, invoiceSerie);
        await _invoiceSaleService.CreateAsync(invoiceSale);

        // Agregar detalles del comprobante.
        var invoiceSaleDetails = _venta.GetInvoiceDetail(invoiceSale.Id);
        await _invoiceSaleDetailService.CreateAsync(invoiceSaleDetails);

        // Agregar Tributos de Factura.
        var tributoSales = _venta.GetTributo(invoiceSale.Id);
        await _tributoSaleService.CreateAsync(tributoSales);

        // Registrar operación de Caja.
        var cashierDetail = new CashierDetail()
        {
            Id = string.Empty,
            CajaDiaria = cajaDiariaId,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Number}",
            Contact = invoiceSale.RznSocialUsuario,
            Remark = _venta.Remark,
            Type = "ENTRADA",
            TypeOperation = TypeOperation.Comprobante,
            FormaPago = _venta.FormaPago,
            Amount = invoiceSale.SumImpVenta
        };
        await _cashierDetailService.CreateAsync(cashierDetail);
        return invoiceSale;
    }

    /// <summary>
    /// Generar serie y número del comprobante de venta.
    /// </summary>
    /// <param name="invoiceSerie">Series de facturación</param>
    /// <param name="invoiceSale">Modelo de Facturación</param>
    /// <param name="docType">Tipo comprobante de venta</param>
    private void GenerateInvoiceSerie(ref InvoiceSerie invoiceSerie, ref InvoiceSale invoiceSale, string docType)
    {
        int numComprobante = 0;
        string THROW_MESSAGE = "Ingresa serie de comprobante!";
        switch (docType)
        {
            case "FACTURA":
                invoiceSale.Serie = invoiceSerie.Factura;
                numComprobante = invoiceSerie.CounterFactura;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante = numComprobante + 1;
                invoiceSerie.CounterFactura = numComprobante;
                break;
            case "BOLETA":
                invoiceSale.Serie = invoiceSerie.Boleta;
                numComprobante = invoiceSerie.CounterBoleta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante = numComprobante + 1;
                invoiceSerie.CounterBoleta = numComprobante;
                break;
            case "NOTA":
                invoiceSale.Serie = invoiceSerie.NotaDeVenta;
                numComprobante = invoiceSerie.CounterNotaDeVenta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante = numComprobante + 1;
                invoiceSerie.CounterNotaDeVenta = numComprobante;
                break;
        }

        invoiceSale.Number = numComprobante.ToString("D8");
    }
}
