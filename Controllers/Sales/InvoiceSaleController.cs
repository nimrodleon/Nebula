using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Services.Common;
using Nebula.Database.Services.Facturador;
using Nebula.Database.Services.Sales;
using Nebula.Database.Dto.Common;
using Nebula.Database.Dto.Sales;
using Nebula.Database.Services;
using Nebula.Database.Models.Common;
using Nebula.Database.Services.Inventory;

namespace Nebula.Controllers.Sales;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class InvoiceSaleController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ConfigurationService _configurationService;
    private readonly CrudOperationService<InvoiceSerie> _invoiceSerieService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;
    private readonly ComprobanteService _comprobanteService;
    private readonly FacturadorService _facturadorService;
    private readonly CreditNoteService _creditNoteService;
    private readonly ValidateStockService _validateStockService;
    private readonly ConsultarValidezComprobanteService _consultarValidezComprobanteService;

    public InvoiceSaleController(ConfigurationService configurationService,
        CrudOperationService<InvoiceSerie> invoiceSerieService, InvoiceSaleService invoiceSaleService,
        InvoiceSaleDetailService invoiceSaleDetailService, TributoSaleService tributoSaleService,
        ComprobanteService comprobanteService, FacturadorService facturadorService, CreditNoteService creditNoteService,
        ValidateStockService validateStockService, IConfiguration configuration,
        ConsultarValidezComprobanteService consultarValidezComprobanteService)
    {
        _configuration = configuration;
        _configurationService = configurationService;
        _invoiceSerieService = invoiceSerieService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _comprobanteService = comprobanteService;
        _facturadorService = facturadorService;
        _creditNoteService = creditNoteService;
        _validateStockService = validateStockService;
        _consultarValidezComprobanteService = consultarValidezComprobanteService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] DateQuery model)
    {
        var invoiceSales = await _invoiceSaleService.GetListAsync(model);
        return Ok(invoiceSales);
    }

    [AllowAnonymous]
    [HttpGet("ReporteMensual")]
    public async Task<IActionResult> ReporteMensual([FromQuery] DateQuery dto)
    {
        var invoiceSeries = await _invoiceSerieService.GetAsync("Name", string.Empty);
        var invoiceSales = await _invoiceSaleService.GetListAsync(dto);
        var creditNotes = await _creditNoteService.GetListAsync(dto);
        string filePath = new ExportarReporteMensual(invoiceSeries, invoiceSales, creditNotes).GuardarCambios();
        FileStream stream = new FileStream(filePath, FileMode.Open);
        return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [HttpGet("Pendientes")]
    public async Task<IActionResult> Pendientes()
    {
        var invoiceSales = await _invoiceSaleService.GetInvoiceSalesPendingAsync();
        return Ok(invoiceSales);
    }

    [HttpPost("BusquedaAvanzada")]
    public async Task<IActionResult> BusquedaAvanzada([FromBody] BuscarComprobanteFormDto dto)
    {
        var invoiceSales = await _invoiceSaleService.BusquedaAvanzadaAsync(dto);
        return Ok(invoiceSales);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ComprobanteDto dto)
    {
        var license = await _configurationService.ValidarAcceso();
        if (!license.Ok) return BadRequest(new { Ok = false, Msg = "Error, Verificar suscripción!" });
        _comprobanteService.SetComprobanteDto(dto);
        var invoiceSale = await _comprobanteService.SaveChangesAsync();
        await _facturadorService.JsonInvoiceParser(invoiceSale.Id);
        await _validateStockService.ValidarInvoiceSale(invoiceSale.Id);
        return Ok(invoiceSale);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var responseInvoiceSale = await _invoiceSaleService.GetInvoiceSaleAsync(id);
        return Ok(responseInvoiceSale);
    }

    [HttpPatch("AnularComprobante/{id}")]
    public async Task<IActionResult> AnularComprobante(string id)
    {
        var responseAnularComprobante = new ResponseAnularComprobante();
        var creditNote = await _creditNoteService.AnulaciónDeLaOperación(id);
        responseAnularComprobante.CreditNote = creditNote;
        responseAnularComprobante.JsonFileCreated = await _facturadorService.CreateCreditNoteJsonFile(creditNote.Id);
        if (responseAnularComprobante.JsonFileCreated)
        {
            responseAnularComprobante.InvoiceSale =
                await _invoiceSaleService.AnularComprobante(creditNote.InvoiceSaleId);
        }

        return Ok(responseAnularComprobante);
    }

    [HttpPatch("SituacionFacturador/{id}")]
    public async Task<IActionResult> SituacionFacturador(string id, [FromBody] SituacionFacturadorDto dto)
    {
        var response = await _invoiceSaleService.SetSituacionFacturador(id, dto);
        return Ok(response);
    }

    [HttpPatch("SaveInControlFolder/{invoiceSaleId}")]
    public async Task<IActionResult> SituacionFacturador(string invoiceSaleId)
    {
        try
        {
            var invoiceSale = await _facturadorService.SaveInvoiceInControlFolder(invoiceSaleId);
            return Ok(invoiceSale);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var invoiceSale = await _invoiceSaleService.GetByIdAsync(id);
        await _invoiceSaleService.RemoveAsync(invoiceSale.Id);
        await _invoiceSaleDetailService.RemoveAsync(invoiceSale.Id);
        await _tributoSaleService.RemoveAsync(invoiceSale.Id);
        return Ok(new { Ok = true, Data = invoiceSale, Msg = "El comprobante de venta ha sido borrado!" });
    }

    [HttpDelete("BorrarArchivosAntiguos/{invoiceSaleId}")]
    public async Task<IActionResult> BorrarArchivos(string invoiceSaleId)
    {
        var invoiceSale = await _facturadorService.BorrarArchivosAntiguosInvoice(invoiceSaleId);
        await _facturadorService.JsonInvoiceParser(invoiceSale.Id);
        return Ok(invoiceSale);
    }

    [AllowAnonymous]
    [HttpGet("GetPdf/{id}")]
    public async Task<IActionResult> GetPdf(string id)
    {
        var configuration = await _configurationService.GetAsync();
        var comprobante = await _invoiceSaleService.GetByIdAsync(id);
        string tipDocu = string.Empty;
        if (comprobante.DocType.Equals("FACTURA")) tipDocu = "01";
        if (comprobante.DocType.Equals("BOLETA")) tipDocu = "03";
        // 20520485750-03-B001-00000015
        string nomArch = $"{configuration.Ruc}-{tipDocu}-{comprobante.Serie}-{comprobante.Number}.pdf";
        string pathPdf = string.Empty;
        var storagePath = _configuration.GetValue<string>("StoragePath");
        if (comprobante.DocumentPath == DocumentPathType.SFS)
        {
            string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
            if (sunatArchivos is null) sunatArchivos = string.Empty;
            string carpetaArchivoSunat = Path.Combine(sunatArchivos, "sfs");
            pathPdf = Path.Combine(carpetaArchivoSunat, "REPO", nomArch);
        }

        if (comprobante.DocumentPath == DocumentPathType.CONTROL)
        {
            if (storagePath is null) storagePath = string.Empty;
            string carpetaArchivoSunat = Path.Combine(storagePath, "sunat");
            string carpetaRepo = Path.Combine(carpetaArchivoSunat, "REPO", comprobante.Year, comprobante.Month);
            pathPdf = Path.Combine(carpetaRepo, nomArch);
        }

        FileStream stream = new FileStream(pathPdf, FileMode.Open);
        return new FileStreamResult(stream, "application/pdf");
    }

    [HttpGet("Ticket/{id}")]
    public async Task<IActionResult> Ticket(string id)
    {
        var ticket = await _invoiceSaleService.GetTicketDto(id);
        return Ok(ticket);
    }

    [AllowAnonymous]
    [HttpGet("ConsultarValidez")]
    public async Task<IActionResult> ConsultarValidez([FromQuery] QueryConsultarValidezComprobante query)
    {
        string pathArchivoZip = await _consultarValidezComprobanteService.CrearArchivosDeValidación(query);
        FileStream stream = new FileStream(pathArchivoZip, FileMode.Open);
        return new FileStreamResult(stream, "application/zip");
    }
}
