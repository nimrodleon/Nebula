using Nebula.Modules.Sales.Models;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Account;

namespace Nebula.Modules.Sales.Comprobantes;

public interface IComprobanteService
{
    Task<InvoiceSaleAndDetails> SaveChangesAsync(ComprobanteDto comprobante);
}

public class ComprobanteService : IComprobanteService
{
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly ITributoSaleService _tributoSaleService;
    private readonly IInvoiceSerieService _invoiceSerieService;
    private readonly IDetallePagoSaleService _detallePagoSaleService;
    private readonly IReceivableService _receivableService;

    public ComprobanteService(IInvoiceSaleService invoiceSaleService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        ITributoSaleService tributoSaleService,
        IInvoiceSerieService invoiceSerieService,
        IDetallePagoSaleService detallePagoSaleService,
        IReceivableService receivableService)
    {
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _invoiceSerieService = invoiceSerieService;
        _detallePagoSaleService = detallePagoSaleService;
        _receivableService = receivableService;
    }

    /// <summary>
    /// Guarda el registro del comprobante en la base de datos.
    /// </summary>
    /// <param name="comprobante">ComprobanteDto</param>
    /// <returns>InvoiceSaleAndDetails</returns>
    public async Task<InvoiceSaleAndDetails> SaveChangesAsync(ComprobanteDto comprobante)
    {
        string companyId = comprobante.Company.Id;
        var invoiceSale = comprobante.GetInvoiceSale();
        var invoiceSerieId = comprobante.Cabecera.InvoiceSerieId;
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, invoiceSerieId);
        comprobante.GenerarSerieComprobante(ref invoiceSerie, ref invoiceSale);
        invoiceSale.InvoiceSerieId = invoiceSerie.Id;

        // agregar Información del comprobante.
        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        await _invoiceSaleService.CreateAsync(invoiceSale);

        // agregar detalles del comprobante.
        var invoiceSaleDetails = comprobante.GetInvoiceSaleDetails(invoiceSale.Id);
        await _invoiceSaleDetailService.CreateManyAsync(invoiceSaleDetails);

        // agregar Tributos de Factura.
        var tributoSales = comprobante.GetTributoSales(invoiceSale.Id);
        await _tributoSaleService.CreateManyAsync(tributoSales);

        // registrar detalle de pago si la operación es a crédito.
        var detallePagos = new List<DetallePagoSale>();
        if (comprobante.DatoPago.FormaPago == FormaPago.Credito)
        {
            if (invoiceSale.DocType == "FACTURA")
            {
                detallePagos = comprobante.GetDetallePagos(invoiceSale.Id);
                if (detallePagos.Count() > 0)
                    await _detallePagoSaleService.InsertManyAsync(detallePagos);
            }

            // registrar cargo.
            Receivable cargo = GenerarCargo(invoiceSale);
            await _receivableService.CreateAsync(cargo);
        }

        var result = new InvoiceSaleAndDetails()
        {
            InvoiceSale = invoiceSale,
            InvoiceSaleDetails = invoiceSaleDetails,
            DetallePagoSale = detallePagos,
        };

        return result;
    }

    /// <summary>
    /// Generar Cargo comprobante a crédito.
    /// </summary>
    /// <param name="invoiceSale">InvoiceSale</param>
    /// <returns>Receivable</returns>
    private Receivable GenerarCargo(InvoiceSale invoiceSale)
    {
        return new Receivable()
        {
            Type = "CARGO",
            CompanyId = invoiceSale.CompanyId,
            ContactId = invoiceSale.ContactId,
            ContactName = invoiceSale.RznSocialUsuario,
            Remark = invoiceSale.Remark,
            InvoiceSale = invoiceSale.Id,
            DocType = invoiceSale.DocType,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Number}",
            FormaPago = "-",
            Cargo = invoiceSale.SumImpVenta,
            Status = "PENDIENTE",
            CreatedAt = DateTime.Now.ToString("yyyy-MM-dd"),
            EndDate = invoiceSale.FecVencimiento,
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
        };
    }
}
