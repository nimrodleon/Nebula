using Nebula.Database.Dto.Sales;
using Nebula.Database.Helpers;
using Nebula.Database.Services.Common;
using Nebula.Modules.Facturador.JsonParser;
using Nebula.Modules.Facturador.Sqlite;
using Nebula.Modules.Sales;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Facturador;

public class FacturadorService
{
    private readonly IConfiguration _configuration;

    // ======================================================================
    private readonly ConfigurationService _configurationService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;

    private readonly DetallePagoSaleService _detallePagoSaleService;

    // ======================================================================
    private readonly CreditNoteService _creditNoteService;

    public FacturadorService(IConfiguration configuration, ConfigurationService configurationService,
        InvoiceSaleService invoiceSaleService, InvoiceSaleDetailService invoiceSaleDetailService,
        TributoSaleService tributoSaleService, CreditNoteService creditNoteService,
        DetallePagoSaleService detallePagoSaleService)
    {
        _configuration = configuration;
        _configurationService = configurationService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _creditNoteService = creditNoteService;
        _detallePagoSaleService = detallePagoSaleService;
    }

    /// <summary>
    /// Mover facturas a la carpeta control.
    /// </summary>
    public async Task<InvoiceSale> SaveInvoiceInControlFolder(string invoiceSaleId)
    {
        var configuration = await _configurationService.GetAsync();
        var invoiceSale = await _invoiceSaleService.GetByIdAsync(invoiceSaleId);
        // configurar nombre del archivo.
        string typeDoc = string.Empty;
        if (invoiceSale.DocType == "BOLETA") typeDoc = "03";
        if (invoiceSale.DocType == "FACTURA") typeDoc = "01";
        string nomArch = $"{configuration.Ruc}-{typeDoc}-{invoiceSale.Serie}-{invoiceSale.Number}";
        string? storagePath = _configuration.GetValue<string>("StoragePath");
        string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
        if (storagePath != null && sunatArchivos != null)
        {
            FacturadorControl.CrearDirectorioControl(storagePath, invoiceSale.Year, invoiceSale.Month);
            FacturadorControl.MoverArchivosControl(storagePath, sunatArchivos, nomArch, invoiceSale.Year,
                invoiceSale.Month);
            FacturadorControl.BorrarArchivosTemporales(sunatArchivos, nomArch);
            // actualizar ruta archivo.
            invoiceSale.DocumentPath = DocumentPathType.CONTROL;
            await _invoiceSaleService.UpdateAsync(invoiceSale.Id, invoiceSale);
            // borrar registro del facturador sunat.
            var dataSource = _configuration.GetValue<string>("BDFacturador");
            var facturador = new FacturadorSqlite(dataSource);
            facturador.BorrarDocumento(nomArch);
        }

        return invoiceSale;
    }

    /// <summary>
    /// Mover notas de crédito a la carpeta control.
    /// </summary>
    public async Task<CreditNote> SaveCreditNoteInControlFolder(string creditNoteId)
    {
        var configuration = await _configurationService.GetAsync();
        var creditNote = await _creditNoteService.GetByIdAsync(creditNoteId);
        // configurar nombre del archivo.
        string nomArch = $"{configuration.Ruc}-07-{creditNote.Serie}-{creditNote.Number}";
        string? storagePath = _configuration.GetValue<string>("StoragePath");
        string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
        if (storagePath != null && sunatArchivos != null)
        {
            FacturadorControl.CrearDirectorioControl(storagePath, creditNote.Year, creditNote.Month);
            FacturadorControl.MoverArchivosControl(storagePath, sunatArchivos, nomArch, creditNote.Year,
                creditNote.Month);
            FacturadorControl.BorrarArchivosTemporales(sunatArchivos, nomArch);
            creditNote.DocumentPath = DocumentPathType.CONTROL;
            await _creditNoteService.UpdateAsync(creditNote.Id, creditNote);
            // borrar registro del facturador sunat.
            var dataSource = _configuration.GetValue<string>("BDFacturador");
            var facturador = new FacturadorSqlite(dataSource);
            facturador.BorrarDocumento(nomArch);
        }

        return creditNote;
    }

    /// <summary>
    /// Borrar antiguos documentos XML del comprobante.
    /// </summary>
    public async Task<InvoiceSale> BorrarArchivosAntiguosInvoice(string invoiceSaleId)
    {
        var configuration = await _configurationService.GetAsync();
        var invoiceSale = await _invoiceSaleService.GetByIdAsync(invoiceSaleId);
        // configurar nombre del archivo.
        string typeDoc = string.Empty;
        if (invoiceSale.DocType == "BOLETA") typeDoc = "03";
        if (invoiceSale.DocType == "FACTURA") typeDoc = "01";
        string nomArch = $"{configuration.Ruc}-{typeDoc}-{invoiceSale.Serie}-{invoiceSale.Number}";
        string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
        if (sunatArchivos != null) FacturadorControl.BorrarTodosLosArchivos(sunatArchivos, nomArch);
        // borrar registro del facturador sunat.
        var dataSource = _configuration.GetValue<string>("BDFacturador");
        var facturador = new FacturadorSqlite(dataSource);
        facturador.BorrarDocumento(nomArch);
        return invoiceSale;
    }

    /// <summary>
    /// Borrar documentos XML antiguos de la nota de crédito.
    /// </summary>
    public async Task<CreditNote> BorrarArchivosAntiguosCreditNote(string creditNoteId)
    {
        var configuration = await _configurationService.GetAsync();
        var creditNote = await _creditNoteService.GetByIdAsync(creditNoteId);
        string nomArch = $"{configuration.Ruc}-07-{creditNote.Serie}-{creditNote.Number}";
        string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
        if (sunatArchivos != null) FacturadorControl.BorrarTodosLosArchivos(sunatArchivos, nomArch);
        // borrar registro del facturador sunat.
        var dataSource = _configuration.GetValue<string>("BDFacturador");
        var facturador = new FacturadorSqlite(dataSource);
        facturador.BorrarDocumento(nomArch);
        return creditNote;
    }

    /// <summary>
    /// Devolver Boleta/Factura Registrada.
    /// </summary>
    /// <param name="invoiceSaleId">Id Venta</param>
    /// <returns>InvoiceSaleDto</returns>
    private async Task<InvoiceSaleDto> GetInvoiceSaleDto(string invoiceSaleId)
    {
        var invoiceSale = await _invoiceSaleService.GetByIdAsync(invoiceSaleId);
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(invoiceSaleId);
        var tributoSales = await _tributoSaleService.GetListAsync(invoiceSaleId);
        var detallePagos = await _detallePagoSaleService.GetListAsync(invoiceSaleId);
        return new InvoiceSaleDto
        {
            InvoiceSale = invoiceSale,
            InvoiceSaleDetails = invoiceSaleDetails,
            TributoSales = tributoSales,
            DetallePagoSales = detallePagos,
        };
    }

    /// <summary>
    /// Generar Boleta/Factura en la carpeta DATA de Facturador.
    /// </summary>
    /// <returns>Existencia del Archivo.</returns>
    public async Task<bool> JsonInvoiceParser(string invoiceSaleId)
    {
        var configuration = await _configurationService.GetAsync();
        var invoiceSaleDto = await GetInvoiceSaleDto(invoiceSaleId);
        // configurar nombre del archivo.
        string typeDoc = string.Empty;
        if (invoiceSaleDto.InvoiceSale.DocType == "BOLETA") typeDoc = "03";
        if (invoiceSaleDto.InvoiceSale.DocType == "FACTURA") typeDoc = "01";
        string fileName =
            $"{configuration.Ruc}-{typeDoc}-{invoiceSaleDto.InvoiceSale.Serie}-{invoiceSaleDto.InvoiceSale.Number}.json";
        string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
        if (sunatArchivos is null) sunatArchivos = string.Empty;
        // generar archivo json.
        string pathBase = Path.Combine(sunatArchivos, "sfs");
        string pathData = Path.Combine(pathBase, "DATA");
        string pathFile = Path.Combine(pathData, fileName);
        if (invoiceSaleDto.InvoiceSale.DocType == "BOLETA")
        {
            var boletaParser = new JsonBoletaParser(invoiceSaleDto);
            boletaParser.CreateJson(pathFile);
        }

        if (invoiceSaleDto.InvoiceSale.DocType == "FACTURA")
        {
            var facturaParser = new JsonFacturaParser(invoiceSaleDto);
            facturaParser.CreateJson(pathFile);
        }

        // actualizar ruta del archivo.
        var invoiceSale = invoiceSaleDto.InvoiceSale;
        invoiceSale.DocumentPath = DocumentPathType.SFS;
        await _invoiceSaleService.UpdateAsync(invoiceSale.Id, invoiceSale);

        return File.Exists(pathFile);
    }

    public async Task<bool> CreateCreditNoteJsonFile(string creditNoteId)
    {
        var configuration = await _configurationService.GetAsync();
        var dto = await _creditNoteService.GetCreditNoteDtoAsync(creditNoteId);
        // configurar nombre del archivo.
        string fileName = $"{configuration.Ruc}-07-{dto.CreditNote.Serie}-{dto.CreditNote.Number}.json";
        string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
        // generar archivo json.
        if (sunatArchivos is null) sunatArchivos = string.Empty;
        string pathBase = Path.Combine(sunatArchivos, "sfs");
        string pathData = Path.Combine(pathBase, "DATA");
        string pathFile = Path.Combine(pathData, fileName);
        var creditNoteParser = new JsonCreditNoteParser(dto);
        creditNoteParser.CreateJson(pathFile);
        // actualizar ruta del archivo.
        var creditNote = dto.CreditNote;
        creditNote.DocumentPath = DocumentPathType.SFS;
        await _creditNoteService.UpdateAsync(creditNote.Id, creditNote);
        return File.Exists(pathFile);
    }
}
