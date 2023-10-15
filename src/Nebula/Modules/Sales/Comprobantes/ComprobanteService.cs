using Nebula.Modules.Sales.Models;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Account;
using Nebula.Common;

namespace Nebula.Modules.Sales.Comprobantes;

public interface IComprobanteService
{
    Task<InvoiceSaleAndDetails> SaveChangesAsync(string companyId, ComprobanteDto comprobante);
}

public class ComprobanteService : IComprobanteService
{
    private readonly ICacheAuthService _cacheAuthService;
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly IInvoiceSerieService _invoiceSerieService;
    private readonly IReceivableService _receivableService;

    public ComprobanteService(ICacheAuthService cacheAuthService,
        IInvoiceSaleService invoiceSaleService, IInvoiceSaleDetailService invoiceSaleDetailService,
        IInvoiceSerieService invoiceSerieService, IReceivableService receivableService)
    {
        _cacheAuthService = cacheAuthService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _invoiceSerieService = invoiceSerieService;
        _receivableService = receivableService;
    }

    /// <summary>
    /// Guarda el registro del comprobante en la base de datos.
    /// </summary>
    /// <param name="comprobante">ComprobanteDto</param>
    /// <returns>InvoiceSaleAndDetails</returns>
    public async Task<InvoiceSaleAndDetails> SaveChangesAsync(string companyId, ComprobanteDto comprobante)
    {
        var company = await _cacheAuthService.GetCompanyByIdAsync(companyId);
        var invoiceSale = comprobante.GetInvoiceSale(company);
        var invoiceSerieId = comprobante.Cabecera.InvoiceSerieId;
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, invoiceSerieId);
        comprobante.GenerarSerieComprobante(ref invoiceSerie, ref invoiceSale);
        invoiceSale.InvoiceSerieId = invoiceSerie.Id;

        // agregar Información del comprobante.
        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        await _invoiceSaleService.CreateAsync(invoiceSale);

        // agregar detalles del comprobante.
        var invoiceSaleDetails = comprobante.GetInvoiceSaleDetails(company, invoiceSale.Id);
        await _invoiceSaleDetailService.CreateManyAsync(invoiceSaleDetails);

        // registrar cargo si la operación es a crédito.
        if (comprobante.FormaPago.Tipo == FormaPago.Credito)
        {
            // registrar cargo.
            Receivable cargo = GenerarCargo(invoiceSale);
            await _receivableService.CreateAsync(cargo);
        }

        var result = new InvoiceSaleAndDetails()
        {
            InvoiceSale = invoiceSale,
            InvoiceSaleDetails = invoiceSaleDetails
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
        string tipoDoc = string.Empty;
        if (invoiceSale.TipoDoc == "01") tipoDoc = "FACTURA";
        if (invoiceSale.TipoDoc == "03") tipoDoc = "BOLETA";
        if (invoiceSale.TipoDoc == "NOTA") tipoDoc = "NOTA";

        return new Receivable()
        {
            Type = "CARGO",
            CompanyId = invoiceSale.CompanyId,
            ContactId = invoiceSale.ContactId,
            ContactName = invoiceSale.Cliente.RznSocial,
            Remark = invoiceSale.Remark,
            InvoiceSale = invoiceSale.Id,
            DocType = tipoDoc,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Correlativo}",
            FormaPago = "-",
            Cargo = invoiceSale.MtoImpVenta,
            Status = "PENDIENTE",
            CreatedAt = DateTime.Now.ToString("yyyy-MM-dd"),
            EndDate = invoiceSale.FecVencimiento,
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
        };
    }
}
