using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common.Dto;
using Nebula.Common.Helpers;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.InvoiceHub.Helpers;
using Nebula.Modules.Sales;
using Nebula.Modules.Sales.Comprobantes;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Helpers;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Invoices.Dto;
using Nebula.Modules.Sales.Notes;

namespace Nebula.Controllers.Sales;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/sales/{companyId}/[controller]")]
[ApiController]
public class InvoiceSaleController : ControllerBase
{
    private readonly IInvoiceSerieService _invoiceSerieService;
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly ITributoSaleService _tributoSaleService;
    private readonly ITributoCreditNoteService _tributoCreditNoteService;
    private readonly IComprobanteService _comprobanteService;
    private readonly ICreditNoteService _creditNoteService;
    private readonly IConsultarValidezComprobanteService _consultarValidezComprobanteService;
    private readonly IInvoiceHubService _invoiceHubService;

    public InvoiceSaleController(
        IInvoiceSerieService invoiceSerieService,
        IInvoiceSaleService invoiceSaleService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        ITributoSaleService tributoSaleService,
        IComprobanteService comprobanteService,
        ICreditNoteService creditNoteService,
        IConsultarValidezComprobanteService consultarValidezComprobanteService,
        ITributoCreditNoteService tributoCreditNoteService,
        IInvoiceHubService invoiceHubService)
    {
        _invoiceSerieService = invoiceSerieService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _comprobanteService = comprobanteService;
        _creditNoteService = creditNoteService;
        _consultarValidezComprobanteService = consultarValidezComprobanteService;
        _tributoCreditNoteService = tributoCreditNoteService;
        _invoiceHubService = invoiceHubService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model)
    {
        var invoiceSales = await _invoiceSaleService.GetListAsync(companyId, model);
        return Ok(invoiceSales);
    }

    [AllowAnonymous]
    [HttpGet("ExcelRegistroVentasF141")]
    public async Task<IActionResult> ExcelRegistroVentasF141(string companyId, [FromQuery] DateQuery dto)
    {
        string[] fieldNames = new string[] { "Name" };
        ParamExcelRegistroVentasF141 param = new ParamExcelRegistroVentasF141
        {
            InvoiceSeries = await _invoiceSerieService.GetFilteredAsync(companyId, fieldNames, string.Empty),
            InvoiceSales = await _invoiceSaleService.GetListAsync(companyId, dto),
            CreditNotes = await _creditNoteService.GetListAsync(companyId, dto),
            TributoSales = await _tributoSaleService.GetTributosMensual(companyId, dto),
            TributoCreditNotes = await _tributoCreditNoteService.GetTributosMensual(companyId, dto)
        };
        // Obtener lista de comprobantes de notas de crédito.
        List<string> seriesComprobante = new List<string>();
        List<string> númerosComprobante = new List<string>();
        param.CreditNotes.ForEach(item =>
        {
            seriesComprobante.Add(item.NumDocAfectado.Split("-")[0].Trim());
            númerosComprobante.Add(item.NumDocAfectado.Split("-")[1].Trim());
        });
        seriesComprobante = seriesComprobante.Distinct().ToList();
        númerosComprobante = númerosComprobante.Distinct().ToList();
        var comprobantesDeNotas = await _invoiceSaleService.GetInvoicesByNumDocs(companyId, seriesComprobante, númerosComprobante);
        param.ComprobantesDeNotas = comprobantesDeNotas;
        // generar archivo excel y enviar como respuesta de solicitud.
        string filePath = new ExcelRegistroVentasF141(param).CrearArchivo();
        FileStream stream = new FileStream(filePath, FileMode.Open);
        return new FileStreamResult(stream, ContentTypeFormat.Excel);
    }

    [HttpGet("Pendientes")]
    public async Task<IActionResult> Pendientes(string companyId)
    {
        var invoiceSales = await _invoiceSaleService.GetInvoiceSalesPendingAsync(companyId);
        return Ok(invoiceSales);
    }

    [HttpPost("BusquedaAvanzada")]
    public async Task<IActionResult> BusquedaAvanzada(string companyId, [FromBody] BuscarComprobanteFormDto dto)
    {
        var invoiceSales = await _invoiceSaleService.BusquedaAvanzadaAsync(companyId, dto);
        return Ok(invoiceSales);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] ComprobanteDto dto)
    {
        var comprobante = await _comprobanteService.SaveChangesAsync(dto);
        var invoiceRequest = InvoiceMapper.MapToInvoiceRequestHub(companyId, comprobante);
        var result = await _invoiceHubService.SendInvoiceAsync(invoiceRequest);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var responseInvoiceSale = await _invoiceSaleService.GetInvoiceSaleAsync(companyId, id);
        return Ok(responseInvoiceSale);
    }

    [HttpPatch("AnularComprobante/{id}")]
    public async Task<IActionResult> AnularComprobante(string companyId, string id)
    {
        var responseAnularComprobante = new ResponseAnularComprobante();
        var creditNote = await _creditNoteService.AnulaciónDeLaOperación(companyId, id);
        responseAnularComprobante.CreditNote = creditNote;
        //responseAnularComprobante.JsonFileCreated = await _facturadorService.CreateCreditNoteJsonFile(creditNote.Id);
        //if (responseAnularComprobante.JsonFileCreated)
        //{
        //    responseAnularComprobante.InvoiceSale =
        //        await _invoiceSaleService.AnularComprobante(creditNote.InvoiceSaleId);
        //}
        return Ok(responseAnularComprobante);
    }

    [HttpPatch("SituacionFacturador/{id}")]
    public async Task<IActionResult> SituacionFacturador(string companyId, string id, [FromBody] SituacionFacturadorDto dto)
    {
        var response = await _invoiceSaleService.SetSituacionFacturador(companyId, id, dto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var invoiceSale = await _invoiceSaleService.GetByIdAsync(companyId, id);
        await _invoiceSaleService.RemoveAsync(companyId, invoiceSale.Id);
        await _invoiceSaleDetailService.RemoveAsync(companyId, invoiceSale.Id);
        await _tributoSaleService.RemoveAsync(companyId, invoiceSale.Id);
        return Ok(new { Ok = true, Data = invoiceSale, Msg = "El comprobante de venta ha sido borrado!" });
    }

    [HttpGet("Ticket/{id}")]
    public async Task<IActionResult> Ticket(string companyId, string id)
    {
        var responseInvoice = await _invoiceSaleService.GetInvoiceSaleAsync(companyId, id);
        var ticket = new TicketDto()
        {
            Company = new Company(),
            InvoiceSale = responseInvoice.InvoiceSale,
            InvoiceSaleDetails = responseInvoice.InvoiceSaleDetails,
            TributoSales = responseInvoice.TributoSales,
        };
        return Ok(ticket);
    }

    [AllowAnonymous]
    [HttpGet("ConsultarValidez")]
    public async Task<IActionResult> ConsultarValidez(string companyId, [FromQuery] QueryConsultarValidezComprobante query)
    {
        string pathArchivoZip = await _consultarValidezComprobanteService.CrearArchivosDeValidación(new Company(), query);
        FileStream stream = new FileStream(pathArchivoZip, FileMode.Open);
        return new FileStreamResult(stream, "application/zip");
    }
}
