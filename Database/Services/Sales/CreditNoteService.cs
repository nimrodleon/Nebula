using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Dto.Sales;
using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Services.Sales;

public class CreditNoteService : CrudOperationService<CreditNote>
{
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;
    private readonly CrudOperationService<InvoiceSerie> _invoiceSerieService;
    // ======================================================================
    private readonly CreditNoteDetailService _creditNoteDetailService;
    private readonly TributoCreditNoteService _tributoCreditNoteService;

    public CreditNoteService(IOptions<DatabaseSettings> options, InvoiceSaleService invoiceSaleService,
        InvoiceSaleDetailService invoiceSaleDetailService, TributoSaleService tributoSaleService, CrudOperationService<InvoiceSerie> invoiceSerieService,
        CreditNoteDetailService creditNoteDetailService, TributoCreditNoteService tributoCreditNoteService) : base(options)
    {
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _invoiceSerieService = invoiceSerieService;
        _creditNoteDetailService = creditNoteDetailService;
        _tributoCreditNoteService = tributoCreditNoteService;
    }

    public async Task<CreditNote> GetCreditNoteByInvoiceSaleIdAsync(string invoiceSaleId) =>
        await _collection.Find(x => x.InvoiceSaleId == invoiceSaleId).FirstOrDefaultAsync();

    public async Task<CreditNote> SetSituacionFacturador(string id, SituacionFacturadorDto dto)
    {
        var creditNote = await GetAsync(id);
        creditNote.SituacionFacturador = $"{dto.Id}:{dto.Nombre}";
        creditNote = await UpdateAsync(creditNote.Id, creditNote);
        return creditNote;
    }

    public async Task<CreditNoteDto> GetCreditNoteDtoAsync(string id)
    {
        var creditNote = await GetAsync(id);
        var creditNoteDetails = await _creditNoteDetailService.GetListAsync(creditNote.Id);
        var tributosCreditNote = await _tributoCreditNoteService.GetListAsync(creditNote.Id);
        return new CreditNoteDto()
        {
            CreditNote = creditNote,
            CreditNoteDetails = creditNoteDetails,
            TributosCreditNote = tributosCreditNote,
        };
    }

    public async Task<CreditNote> AnulaciónDeLaOperación(string id)
    {
        var invoiceSale = await _invoiceSaleService.GetAsync(id);
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(invoiceSale.Id);
        var tributoSales = await _tributoSaleService.GetListAsync(invoiceSale.Id);
        var invoiceSerie = await _invoiceSerieService.GetAsync(invoiceSale.InvoiceSerieId);
        var creditNote = GetCreditNote(invoiceSale);
        GenerarSerieComprobante(ref invoiceSerie, ref creditNote);

        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        await CreateAsync(creditNote);

        var creditNoteDetails = GetCreditNoteDetails(creditNote.Id, invoiceSaleDetails);
        await _creditNoteDetailService.InsertManyAsync(creditNoteDetails);

        var tributosCreditNote = GetTributosCreditNote(creditNote.Id, tributoSales);
        await _tributoCreditNoteService.InsertManyAsync(tributosCreditNote);

        return creditNote;
    }

    private CreditNote GetCreditNote(InvoiceSale invoiceSale)
    {
        string tipDocAfectado = string.Empty;
        if (invoiceSale.DocType == "FACTURA") tipDocAfectado = "01";
        if (invoiceSale.DocType == "BOLETA") tipDocAfectado = "03";

        return new CreditNote()
        {
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
        };
    }

    private List<CreditNoteDetail> GetCreditNoteDetails(string creditNoteId, List<InvoiceSaleDetail> invoiceSaleDetails)
    {
        var creditNoteDetails = new List<CreditNoteDetail>();
        invoiceSaleDetails.ForEach(item =>
        {
            creditNoteDetails.Add(new CreditNoteDetail()
            {
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

        creditNote.Number = numComprobante.ToString("D8");
    }
}
