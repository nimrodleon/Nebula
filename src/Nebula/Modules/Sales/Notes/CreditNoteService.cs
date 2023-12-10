using MongoDB.Driver;
using Nebula.Common;
using Nebula.Common.Dto;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Models;
using Nebula.Modules.Sales.Notes.Dto;

namespace Nebula.Modules.Sales.Notes;

public interface ICreditNoteService : ICrudOperationService<CreditNote>
{
    Task<List<CreditNote>> GetListAsync(string companyId, DateQuery query);
    Task<CreditNote> GetCreditNoteByInvoiceSaleIdAsync(string companyId, string invoiceSaleId);
    Task<List<CreditNote>> GetCreditNotesByMonthAndYear(string companyId, string month, string year);
    Task<List<CreditNote>> GetCreditNotesByDate(string companyId, string date);
    Task<CreditNoteDto> GetCreditNoteDtoAsync(string companyId, string CreditNoteId);
    Task<List<CreditNote>> GetCreditNotesPendingAsync(string companyId);
    Task<InvoiceCancellationResponse> InvoiceCancellation(string companyId, string invoiceSaleId);
    void GenerarSerieComprobante(ref InvoiceSerie invoiceSerie, ref CreditNote creditNote);
}

public class CreditNoteService : CrudOperationService<CreditNote>, ICreditNoteService
{
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly IInvoiceSerieService _invoiceSerieService;
    private readonly ICreditNoteDetailService _creditNoteDetailService;

    public CreditNoteService(MongoDatabaseService mongoDatabase,
        IInvoiceSaleService invoiceSaleService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        IInvoiceSerieService invoiceSerieService,
        ICreditNoteDetailService creditNoteDetailService) : base(mongoDatabase)
    {
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _invoiceSerieService = invoiceSerieService;
        _creditNoteDetailService = creditNoteDetailService;
    }

    public async Task<List<CreditNote>> GetListAsync(string companyId, DateQuery query)
    {
        var builder = Builders<CreditNote>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Month, query.Month), builder.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<CreditNote>().Descending("$natural"))
            .ToListAsync();
    }

    public async Task<CreditNote> GetCreditNoteByInvoiceSaleIdAsync(string companyId, string invoiceSaleId) =>
        await _collection.Find(x => x.CompanyId == companyId && x.InvoiceSaleId == invoiceSaleId).FirstOrDefaultAsync();

    /// <summary>
    /// Obtener notas de crédito por mes y año.
    /// </summary>
    /// <param name="month">mes</param>
    /// <param name="year">año</param>
    /// <returns>Lista de notas de crédito</returns>
    public async Task<List<CreditNote>> GetCreditNotesByMonthAndYear(string companyId, string month, string year)
    {
        var builder = Builders<CreditNote>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Month, month), builder.Eq(x => x.Year, year));
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Obtener notas de crédito por fecha.
    /// </summary>
    /// <param name="date">fecha de emisión del comprobante</param>
    /// <returns>Lista de notas de crédito</returns>
    public async Task<List<CreditNote>> GetCreditNotesByDate(string companyId, string date)
    {
        var builder = Builders<CreditNote>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.FechaEmision, date));
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Retorna los datos completos de una Nota de crédito.
    /// </summary>
    /// <param name="id">Identificador Nota de Crédito</param>
    /// <returns>CreditNoteDto</returns>
    public async Task<CreditNoteDto> GetCreditNoteDtoAsync(string companyId, string CreditNoteId)
    {
        var creditNote = await GetByIdAsync(companyId, CreditNoteId);
        var creditNoteDetails = await _creditNoteDetailService.GetListAsync(companyId, creditNote.Id);
        return new CreditNoteDto()
        {
            CreditNote = creditNote,
            CreditNoteDetails = creditNoteDetails,
        };
    }

    public async Task<List<CreditNote>> GetCreditNotesPendingAsync(string companyId)
    {
        var builder = Builders<CreditNote>.Filter;

        // Verifica si el campo BillingResponse no existe o BillingResponse.Success es false
        var filter = builder.Or(
            builder.Not(builder.Exists(x => x.BillingResponse)),
            builder.Eq("BillingResponse.Success", false)
        ) & builder.Eq(x => x.CompanyId, companyId);

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<InvoiceCancellationResponse> InvoiceCancellation(string companyId, string invoiceSaleId)
    {
        var invoiceSale = await _invoiceSaleService.GetByIdAsync(companyId, invoiceSaleId);
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(companyId, invoiceSale.Id);
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, invoiceSale.InvoiceSerieId);
        var creditNote = GetCreditNote(invoiceSale);
        GenerarSerieComprobante(ref invoiceSerie, ref creditNote);

        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        creditNote.InvoiceSerieId = invoiceSerie.Id;
        await CreateAsync(creditNote);

        var creditNoteDetails = GetCreditNoteDetails(creditNote.Id, invoiceSaleDetails);
        await _creditNoteDetailService.InsertManyAsync(creditNoteDetails);

        return new InvoiceCancellationResponse()
        {
            InvoiceSale = invoiceSale,
            CreditNote = creditNote,
            CreditNoteDetail = creditNoteDetails,
        };
    }

    private CreditNote GetCreditNote(InvoiceSale invoiceSale)
    {
        return new CreditNote()
        {
            CompanyId = invoiceSale.CompanyId,
            InvoiceSaleId = invoiceSale.Id,
            InvoiceSerieId = invoiceSale.InvoiceSerieId,
            TipoDoc = "07",
            FechaEmision = DateTime.Now.ToString("yyyy-MM-dd"),
            ContactId = invoiceSale.ContactId,
            Cliente = invoiceSale.Cliente,
            TipoMoneda = invoiceSale.TipoMoneda,
            CodMotivo = "01",
            DesMotivo = "ANULACIÓN DE LA OPERACIÓN",
            TipDocAfectado = invoiceSale.TipoDoc,
            NumDocfectado = $"{invoiceSale.Serie}-{invoiceSale.Correlativo}",
            MtoOperGravadas = invoiceSale.MtoOperGravadas,
            MtoOperInafectas = invoiceSale.MtoOperInafectas,
            MtoOperExoneradas = invoiceSale.MtoOperExoneradas,
            MtoIGV = invoiceSale.MtoIGV,
            TotalImpuestos = invoiceSale.TotalImpuestos,
            ValorVenta = invoiceSale.ValorVenta,
            SubTotal = invoiceSale.SubTotal,
            MtoImpVenta = invoiceSale.MtoImpVenta,
            TotalEnLetras = invoiceSale.TotalEnLetras,
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
        };
    }

    private List<CreditNoteDetail> GetCreditNoteDetails(string creditNoteId, List<InvoiceSaleDetail> invoiceSaleDetails)
    {
        var creditNoteDetails = new List<CreditNoteDetail>();
        invoiceSaleDetails.ForEach(item =>
        {
            creditNoteDetails.Add(new CreditNoteDetail()
            {
                CompanyId = item.CompanyId,
                CreditNoteId = creditNoteId,
                WarehouseId = item.WarehouseId,
                TipoItem = item.TipoItem,
                Unidad = item.Unidad,
                Cantidad = item.Cantidad,
                CodProducto = item.CodProducto,
                Description = item.Description,
                MtoValorUnitario = item.MtoValorUnitario,
                MtoBaseIgv = item.MtoBaseIgv,
                PorcentajeIgv = item.PorcentajeIgv,
                Igv = item.Igv,
                TipAfeIgv = item.TipAfeIgv,
                TotalImpuestos = item.TotalImpuestos,
                MtoPrecioUnitario = item.MtoPrecioUnitario,
                MtoValorVenta = item.MtoValorVenta,
                RecordType = item.RecordType,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM"),
            });
        });
        return creditNoteDetails;
    }

    public void GenerarSerieComprobante(ref InvoiceSerie invoiceSerie, ref CreditNote creditNote)
    {
        int numComprobante = 0;
        string THROW_MESSAGE = "Ingresa serie de comprobante!";
        switch (creditNote.TipDocAfectado)
        {
            case "01": // FACTURA.
                creditNote.Serie = invoiceSerie.CreditNoteFactura;
                numComprobante = invoiceSerie.CounterCreditNoteFactura;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante += 1;
                invoiceSerie.CounterCreditNoteFactura = numComprobante;
                break;
            case "03": // BOLETA.
                creditNote.Serie = invoiceSerie.CreditNoteBoleta;
                numComprobante = invoiceSerie.CounterCreditNoteBoleta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante += 1;
                invoiceSerie.CounterCreditNoteBoleta = numComprobante;
                break;
        }

        creditNote.Correlativo = numComprobante.ToString();
    }

}
