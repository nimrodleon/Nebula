using Nebula.Common;
using Nebula.Modules.Sales.Models;
using Nebula.Modules.Sales;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Configurations;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Sales.Dto;

namespace Nebula.Modules.Cashier;

public interface ICashierSaleService
{
    void SetComprobanteDto(ComprobanteDto dto);
    Task<InvoiceSale> SaveChangesAsync(string cajaDiariaId);
}

/// <summary>
/// Servicio para gestionar ventas.
/// </summary>
public class CashierSaleService : ICashierSaleService
{
    private readonly IConfigurationService _configurationService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;
    private readonly CrudOperationService<InvoiceSerie> _invoiceSerieService;
    private readonly CashierDetailService _cashierDetailService;
    private readonly ReceivableService _receivableService;
    private readonly CajaDiariaService _cajaDiariaService;
    private readonly DetallePagoSaleService _detallePagoSaleService;

    public CashierSaleService(IConfigurationService configurationService,
        InvoiceSaleService invoiceSaleService, InvoiceSaleDetailService invoiceSaleDetailService,
        TributoSaleService tributoSaleService, CrudOperationService<InvoiceSerie> invoiceSerieService,
        CashierDetailService cashierDetailService, ReceivableService receivableService,
        CajaDiariaService cajaDiariaService, DetallePagoSaleService detallePagoSaleService)
    {
        _configurationService = configurationService;
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
            CajaDiaria = cajaDiaria.Id,
            InvoiceSale = invoiceSale.Id,
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
            var cargo = GenerarCargo(invoiceSale, cajaDiaria, configuration.DiasPlazo);
            await _receivableService.CreateAsync(cargo);
        }

        return invoiceSale;
    }

    private Receivable GenerarCargo(InvoiceSale invoiceSale, CajaDiaria cajaDiaria, int diasPlazo)
    {
        return new Receivable()
        {
            Type = "CARGO",
            ContactId = comprobanteDto.Cabecera.ContactId,
            ContactName = comprobanteDto.Cabecera.RznSocialUsuario,
            Remark = comprobanteDto.Cabecera.Remark,
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
