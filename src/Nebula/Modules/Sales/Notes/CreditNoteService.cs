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
    Task<InvoiceCancellationResponse> InvoiceCancellation(string companyId, string invoiceSaleId);
    void GenerarSerieComprobante(ref InvoiceSerie invoiceSerie, ref CreditNote creditNote);
}

public class CreditNoteService : CrudOperationService<CreditNote>, ICreditNoteService
{
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly ITributoSaleService _tributoSaleService;
    private readonly IInvoiceSerieService _invoiceSerieService;

    // ======================================================================
    private readonly ICreditNoteDetailService _creditNoteDetailService;
    private readonly ITributoCreditNoteService _tributoCreditNoteService;

    public CreditNoteService(MongoDatabaseService mongoDatabase,
        IInvoiceSaleService invoiceSaleService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        ITributoSaleService tributoSaleService,
        IInvoiceSerieService invoiceSerieService,
        ICreditNoteDetailService creditNoteDetailService,
        ITributoCreditNoteService tributoCreditNoteService) : base(mongoDatabase)
    {
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _invoiceSerieService = invoiceSerieService;
        _creditNoteDetailService = creditNoteDetailService;
        _tributoCreditNoteService = tributoCreditNoteService;
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
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.FecEmision, date));
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
        var tributosCreditNote = await _tributoCreditNoteService.GetListAsync(companyId, creditNote.Id);
        return new CreditNoteDto()
        {
            CreditNote = creditNote,
            CreditNoteDetails = creditNoteDetails,
            TributosCreditNote = tributosCreditNote,
        };
    }

    public async Task<InvoiceCancellationResponse> InvoiceCancellation(string companyId, string invoiceSaleId)
    {
        var invoiceSale = await _invoiceSaleService.GetByIdAsync(companyId, invoiceSaleId);
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(companyId, invoiceSale.Id);
        var tributoSales = await _tributoSaleService.GetListAsync(companyId, invoiceSale.Id);
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, invoiceSale.InvoiceSerieId);
        var creditNote = GetCreditNote(invoiceSale);
        GenerarSerieComprobante(ref invoiceSerie, ref creditNote);

        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        creditNote.InvoiceSerieId = invoiceSerie.Id;
        await CreateAsync(creditNote);

        var creditNoteDetails = GetCreditNoteDetails(creditNote.Id, invoiceSaleDetails);
        await _creditNoteDetailService.InsertManyAsync(creditNoteDetails);

        var tributosCreditNote = GetTributosCreditNote(creditNote.Id, tributoSales);
        await _tributoCreditNoteService.InsertManyAsync(tributosCreditNote);

        return new InvoiceCancellationResponse()
        {
            InvoiceSale = invoiceSale,
            CreditNote = creditNote,
            CreditNoteDetail = creditNoteDetails,
        };
    }

    private CreditNote GetCreditNote(InvoiceSale invoiceSale)
    {
        string tipDocAfectado = string.Empty;
        if (invoiceSale.DocType == "FACTURA") tipDocAfectado = "01";
        if (invoiceSale.DocType == "BOLETA") tipDocAfectado = "03";

        return new CreditNote()
        {
            CompanyId = invoiceSale.CompanyId,
            InvoiceSaleId = invoiceSale.Id,
            TipOperacion = invoiceSale.TipOperacion,
            FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
            HorEmision = DateTime.Now.ToString("HH:mm:ss"),
            CodLocalEmisor = invoiceSale.CodLocalEmisor,
            TipDocUsuario = invoiceSale.TipDocUsuario,
            NumDocUsuario = invoiceSale.NumDocUsuario,
            RznSocialUsuario = invoiceSale.RznSocialUsuario,
            TipMoneda = invoiceSale.TipMoneda,
            CodMotivo = "01",
            DesMotivo = "ANULACIÓN DE LA OPERACIÓN",
            TipDocAfectado = tipDocAfectado,
            NumDocAfectado = $"{invoiceSale.Serie}-{invoiceSale.Number}",
            SumTotTributos = invoiceSale.SumTotTributos,
            SumTotValVenta = invoiceSale.SumTotValVenta,
            SumPrecioVenta = invoiceSale.SumPrecioVenta,
            SumImpVenta = invoiceSale.SumImpVenta,
            // DIRECCIÓN_DEL_CLIENTE!
            CodUbigeoCliente = invoiceSale.CodUbigeoCliente,
            DesDireccionCliente = invoiceSale.DesDireccionCliente,
            TotalEnLetras = invoiceSale.TotalEnLetras,
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
                TipoItem = item.TipoItem,
                CodUnidadMedida = item.CodUnidadMedida,
                CtdUnidadItem = item.CtdUnidadItem,
                CodProducto = item.CodProducto,
                CodProductoSunat = item.CodProductoSunat,
                DesItem = item.DesItem,
                MtoValorUnitario = item.MtoValorUnitario,
                SumTotTributosItem = item.SumTotTributosItem,
                CodTriIgv = item.CodTriIgv,
                MtoIgvItem = item.MtoIgvItem,
                MtoBaseIgvItem = item.MtoBaseIgvItem,
                NomTributoIgvItem = item.NomTributoIgvItem,
                CodTipTributoIgvItem = item.CodTipTributoIgvItem,
                TipAfeIgv = item.TipAfeIgv,
                PorIgvItem = item.PorIgvItem,
                CodTriIcbper = item.CodTriIcbper,
                MtoTriIcbperItem = item.MtoTriIcbperItem,
                CtdBolsasTriIcbperItem = item.CtdBolsasTriIcbperItem,
                NomTributoIcbperItem = item.NomTributoIcbperItem,
                CodTipTributoIcbperItem = item.CodTipTributoIcbperItem,
                MtoTriIcbperUnidad = item.MtoTriIcbperUnidad,
                MtoPrecioVentaUnitario = item.MtoPrecioVentaUnitario,
                MtoValorVentaItem = item.MtoValorVentaItem,
                MtoValorReferencialUnitario = item.MtoValorReferencialUnitario,
            });
        });
        return creditNoteDetails;
    }

    private List<TributoCreditNote> GetTributosCreditNote(string creditNoteId, List<TributoSale> tributoSales)
    {
        var tributosCreditNote = new List<TributoCreditNote>();
        tributoSales.ForEach(item =>
        {
            tributosCreditNote.Add(new TributoCreditNote()
            {
                CreditNoteId = creditNoteId,
                IdeTributo = item.IdeTributo,
                NomTributo = item.NomTributo,
                CodTipTributo = item.CodTipTributo,
                MtoBaseImponible = item.MtoBaseImponible,
                MtoTributo = item.MtoTributo,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM"),
            });
        });
        return tributosCreditNote;
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

        creditNote.Number = numComprobante.ToString();
    }

}
