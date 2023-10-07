using Nebula.Common;
using Nebula.Modules.Sales.Models;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Cashier;

public interface ICashierSaleService
{
    Task<InvoiceSale> SaveChangesAsync(ComprobanteDto comprobanteDto, string cajaDiariaId);
}

/// <summary>
/// Servicio para gestionar ventas.
/// </summary>
public class CashierSaleService : ICashierSaleService
{
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly ITributoSaleService _tributoSaleService;
    private readonly ICrudOperationService<InvoiceSerie> _invoiceSerieService;
    private readonly ICashierDetailService _cashierDetailService;
    private readonly IReceivableService _receivableService;
    private readonly ICajaDiariaService _cajaDiariaService;
    private readonly IDetallePagoSaleService _detallePagoSaleService;

    public CashierSaleService(
        IInvoiceSaleService invoiceSaleService, IInvoiceSaleDetailService invoiceSaleDetailService,
        ITributoSaleService tributoSaleService, ICrudOperationService<InvoiceSerie> invoiceSerieService,
        ICashierDetailService cashierDetailService, IReceivableService receivableService,
        ICajaDiariaService cajaDiariaService, IDetallePagoSaleService detallePagoSaleService)
    {
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _invoiceSerieService = invoiceSerieService;
        _cashierDetailService = cashierDetailService;
        _receivableService = receivableService;
        _cajaDiariaService = cajaDiariaService;
        _detallePagoSaleService = detallePagoSaleService;
    }

    /// <summary>
    /// Guardar comprobante de venta rápida.
    /// </summary>
    public async Task<InvoiceSale> SaveChangesAsync(ComprobanteDto comprobanteDto, string cajaDiariaId)
    {
        var invoiceSale = comprobanteDto.GetInvoiceSale();
        var invoiceSerieId = comprobanteDto.Cabecera.InvoiceSerieId;
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(invoiceSerieId);
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

        // registrar operación de Caja.
        var cajaDiaria = await _cajaDiariaService.GetByIdAsync(cajaDiariaId);
        var cashierDetail = new CashierDetail()
        {
            CajaDiariaId = cajaDiaria.Id,
            InvoiceSaleId = invoiceSale.Id,
            DocType = invoiceSale.DocType,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Number}",
            ContactId = invoiceSale.ContactId,
            ContactName = invoiceSale.RznSocialUsuario,
            Remark = comprobanteDto.Cabecera.Remark,
            TypeOperation = TypeOperationCaja.ComprobanteDeVenta,
            FormaPago = comprobanteDto.DatoPago.FormaPago,
            Amount = invoiceSale.SumImpVenta
        };
        await _cashierDetailService.CreateAsync(cashierDetail);

        // registrar cargo y detalle de pago si la operación es a crédito.
        if (comprobanteDto.DatoPago.FormaPago == FormaPago.Credito)
        {
            if (invoiceSale.DocType == "FACTURA")
            {
                var detallePagos = comprobanteDto.GetDetallePagos(invoiceSale.Id);
                if (detallePagos.Count() > 0) await _detallePagoSaleService.InsertManyAsync(detallePagos);
            }
            var cargo = GenerarCargo(invoiceSale, cajaDiaria, comprobanteDto.Company.DiasPlazo);
            await _receivableService.CreateAsync(cargo);
        }

        return invoiceSale;
    }

    private Receivable GenerarCargo(InvoiceSale invoiceSale, CajaDiaria cajaDiaria, int diasPlazo)
    {
        return new Receivable()
        {
            CompanyId = invoiceSale.CompanyId,
            Type = "CARGO",
            ContactId = invoiceSale.ContactId,
            ContactName = invoiceSale.RznSocialUsuario,
            Remark = invoiceSale.Remark,
            InvoiceSale = invoiceSale.Id,
            DocType = invoiceSale.DocType,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Number}",
            FormaPago = "-",
            Cargo = invoiceSale.SumImpVenta,
            Status = "PENDIENTE",
            CajaDiaria = cajaDiaria.Id,
            Terminal = cajaDiaria.Terminal,
            CreatedAt = DateTime.Now.ToString("yyyy-MM-dd"),
            EndDate = DateTime.Now.AddDays(diasPlazo).ToString("yyyy-MM-dd"),
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
        };
    }
}
