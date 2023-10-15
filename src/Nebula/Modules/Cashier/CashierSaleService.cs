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
    Task<InvoiceSale> SaveChangesAsync(Company company, ComprobanteDto comprobanteDto, string cajaDiariaId);
}

/// <summary>
/// Servicio para gestionar ventas.
/// </summary>
public class CashierSaleService : ICashierSaleService
{
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly ICrudOperationService<InvoiceSerie> _invoiceSerieService;
    private readonly ICashierDetailService _cashierDetailService;
    private readonly IReceivableService _receivableService;
    private readonly ICajaDiariaService _cajaDiariaService;

    public CashierSaleService(
        IInvoiceSaleService invoiceSaleService, IInvoiceSaleDetailService invoiceSaleDetailService,
         ICrudOperationService<InvoiceSerie> invoiceSerieService,
        ICashierDetailService cashierDetailService, IReceivableService receivableService,
        ICajaDiariaService cajaDiariaService)
    {
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _invoiceSerieService = invoiceSerieService;
        _cashierDetailService = cashierDetailService;
        _receivableService = receivableService;
        _cajaDiariaService = cajaDiariaService;
    }

    /// <summary>
    /// Guardar comprobante de venta rápida.
    /// </summary>
    public async Task<InvoiceSale> SaveChangesAsync( Company company, ComprobanteDto comprobanteDto, string cajaDiariaId)
    {
        var invoiceSale = comprobanteDto.GetInvoiceSale(company);
        var invoiceSerieId = comprobanteDto.Cabecera.InvoiceSerieId;
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(invoiceSerieId);
        comprobanteDto.GenerarSerieComprobante(ref invoiceSerie, ref invoiceSale);
        invoiceSale.InvoiceSerieId = invoiceSerie.Id;

        // agregar Información del comprobante.
        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        await _invoiceSaleService.CreateAsync(invoiceSale);

        // agregar detalles del comprobante.
        var invoiceSaleDetails = comprobanteDto.GetInvoiceSaleDetails(company, invoiceSale.Id);
        await _invoiceSaleDetailService.CreateManyAsync(invoiceSaleDetails);

        // registrar operación de Caja.
        var cajaDiaria = await _cajaDiariaService.GetByIdAsync(cajaDiariaId);
        var cashierDetail = new CashierDetail()
        {
            CajaDiariaId = cajaDiaria.Id,
            InvoiceSaleId = invoiceSale.Id,
            DocType = invoiceSale.TipoDoc,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Correlativo}",
            ContactId = invoiceSale.ContactId,
            ContactName = invoiceSale.Cliente.RznSocial,
            Remark = comprobanteDto.Cabecera.Remark,
            TypeOperation = TypeOperationCaja.ComprobanteDeVenta,
            FormaPago = comprobanteDto.FormaPago.Tipo,
            Amount = invoiceSale.MtoImpVenta
        };
        await _cashierDetailService.CreateAsync(cashierDetail);

        // registrar cargo si la operación es a crédito.
        if (comprobanteDto.FormaPago.Tipo == FormaPago.Credito)
        {            
            var cargo = GenerarCargo(invoiceSale, cajaDiaria, company.DiasPlazo);
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
            ContactName = invoiceSale.Cliente.RznSocial,
            Remark = invoiceSale.Remark,
            InvoiceSale = invoiceSale.Id,
            DocType = invoiceSale.TipoDoc,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Correlativo}",
            FormaPago = "-",
            Cargo = invoiceSale.MtoImpVenta,
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
