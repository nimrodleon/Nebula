using MongoDB.Driver;
using Nebula.Common;
using Nebula.Common.Dto;
using Nebula.Modules.Configurations;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Configurations.Warehouses;
using Nebula.Modules.Facturador.Helpers;
using Nebula.Modules.Facturador.XmlDigest;
using Nebula.Modules.Sales.Helpers;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Models;
using Nebula.Modules.Sales.Notes.Dto;

namespace Nebula.Modules.Sales.Notes;

public interface ICreditNoteService : ICrudOperationService<CreditNote>
{
    Task<List<CreditNote>> GetListAsync(DateQuery query);
    Task<CreditNote> GetCreditNoteByInvoiceSaleIdAsync(string invoiceSaleId);
    Task<List<CreditNote>> GetCreditNotesByMonthAndYear(string month, string year);
    Task<List<CreditNote>> GetCreditNotesByDate(string date);
    Task<CreditNote> SetSituacionFacturador(string id, SituacionFacturadorDto dto);
    Task<CreditNoteDto> GetCreditNoteDtoAsync(string id);
    Task<CreditNote> AnulaciónDeLaOperación(string id);
    void GenerarSerieComprobante(ref InvoiceSerie invoiceSerie, ref CreditNote creditNote);
    Task<PrintCreditNoteDto> GetPrintCreditNoteDto(string creditNoteId);
}

public class CreditNoteService : CrudOperationService<CreditNote>, ICreditNoteService
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationService _configurationService;
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly ITributoSaleService _tributoSaleService;

    private readonly IInvoiceSerieService _invoiceSerieService;

    // ======================================================================
    private readonly ICreditNoteDetailService _creditNoteDetailService;
    private readonly ITributoCreditNoteService _tributoCreditNoteService;

    public CreditNoteService(MongoDatabaseService mongoDatabase,
        IConfiguration configuration,
        IConfigurationService configurationService,
        IInvoiceSaleService invoiceSaleService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        ITributoSaleService tributoSaleService,
        IInvoiceSerieService invoiceSerieService,
        ICreditNoteDetailService creditNoteDetailService,
        ITributoCreditNoteService tributoCreditNoteService) : base(mongoDatabase)
    {
        _configurationService = configurationService;
        _configuration = configuration;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _invoiceSerieService = invoiceSerieService;
        _creditNoteDetailService = creditNoteDetailService;
        _tributoCreditNoteService = tributoCreditNoteService;
    }

    public async Task<List<CreditNote>> GetListAsync(DateQuery query)
    {
        var builder = Builders<CreditNote>.Filter;
        var filter = builder.And(builder.Eq(x => x.Month, query.Month), builder.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<CreditNote>().Descending("$natural"))
            .ToListAsync();
    }

    public async Task<CreditNote> GetCreditNoteByInvoiceSaleIdAsync(string invoiceSaleId) =>
        await _collection.Find(x => x.InvoiceSaleId == invoiceSaleId).FirstOrDefaultAsync();

    /// <summary>
    /// Obtener notas de crédito por mes y año.
    /// </summary>
    /// <param name="month">mes</param>
    /// <param name="year">año</param>
    /// <returns>Lista de notas de crédito</returns>
    public async Task<List<CreditNote>> GetCreditNotesByMonthAndYear(string month, string year)
    {
        var builder = Builders<CreditNote>.Filter;
        var filter = builder.And(builder.Eq(x => x.Month, month),
            builder.Eq(x => x.Year, year));
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Obtener notas de crédito por fecha.
    /// </summary>
    /// <param name="date">fecha de emisión del comprobante</param>
    /// <returns>Lista de notas de crédito</returns>
    public async Task<List<CreditNote>> GetCreditNotesByDate(string date)
    {
        var builder = Builders<CreditNote>.Filter;
        var filter = builder.Eq(x => x.FecEmision, date);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<CreditNote> SetSituacionFacturador(string id, SituacionFacturadorDto dto)
    {
        var creditNote = await GetByIdAsync(id);
        creditNote.SituacionFacturador = $"{dto.Id}:{dto.Nombre}";
        creditNote = await UpdateAsync(creditNote.Id, creditNote);
        return creditNote;
    }

    /// <summary>
    /// Retorna los datos completos de una Nota de crédito.
    /// </summary>
    /// <param name="id">Identificador Nota de Crédito</param>
    /// <returns>CreditNoteDto</returns>
    public async Task<CreditNoteDto> GetCreditNoteDtoAsync(string id)
    {
        var creditNote = await GetByIdAsync(id);
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
        var invoiceSale = await _invoiceSaleService.GetByIdAsync(id);
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(invoiceSale.Id);
        var tributoSales = await _tributoSaleService.GetListAsync(invoiceSale.Id);
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(invoiceSale.InvoiceSerieId);
        var creditNote = GetCreditNote(invoiceSale);
        GenerarSerieComprobante(ref invoiceSerie, ref creditNote);

        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        creditNote.InvoiceSerieId = invoiceSerie.Id;
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
            // DIRECCIÓN_DEL_CLIENTE!
            CodUbigeoCliente = invoiceSale.CodUbigeoCliente,
            DesDireccionCliente = invoiceSale.DesDireccionCliente,
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

    /// <summary>
    /// Retorna datos de nota de crédito para Imprimir.
    /// </summary>
    /// <param name="creditNoteId">Identificador de la Nota de crédito</param>
    /// <returns>PrintCreditNoteDto</returns>
    public async Task<PrintCreditNoteDto> GetPrintCreditNoteDto(string creditNoteId)
    {
        // Cargar datos básicos.
        var configuration = await _configurationService.GetAsync();
        var creditNoteDto = await GetCreditNoteDtoAsync(creditNoteId);
        var creditNote = creditNoteDto.CreditNote;
        // configurar nombre de archivo XML.
        // 20520485750-07-BC01-00000015
        string nomArch = $"{configuration.Ruc}-07-{creditNote.Serie}-{creditNote.Number}.xml";
        string pathXml = string.Empty;
        var storagePath = _configuration.GetValue<string>("StoragePath");
        // abrir en la ruta del facturador.
        if (creditNote.DocumentPath == DocumentPathType.SFS)
        {
            string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
            if (sunatArchivos is null) sunatArchivos = string.Empty;
            string carpetaArchivoSunat = Path.Combine(sunatArchivos, "sfs");
            pathXml = Path.Combine(carpetaArchivoSunat, "FIRMA", nomArch);
        }

        // abrir en la ruta de la carpeta control.
        if (creditNote.DocumentPath == DocumentPathType.CONTROL)
        {
            if (storagePath is null) storagePath = string.Empty;
            string carpetaArchivoSunat = Path.Combine(storagePath, "facturador");
            string carpetaRepo = Path.Combine(carpetaArchivoSunat, "FIRMA", creditNote.Year, creditNote.Month);
            pathXml = Path.Combine(carpetaRepo, nomArch);
        }

        // establecer valores de retorno.
        var printCreditNote = new PrintCreditNoteDto
        {
            Configuration = configuration,
            CreditNote = creditNoteDto.CreditNote,
            CreditNoteDetails = creditNoteDto.CreditNoteDetails,
            TributosCreditNote = creditNoteDto.TributosCreditNote
        };
        LeerDigestValue digest = new LeerDigestValue();
        printCreditNote.DigestValue = digest.GetCreditNoteXmlValue(pathXml);
        printCreditNote.TotalEnLetras = new NumberToLetters(creditNote.SumImpVenta).ToString();
        return printCreditNote;
    }
}
