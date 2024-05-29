using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common.Dto;
using Nebula.Common.Helpers;
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
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Account;
using Nebula.Modules.Sales.Models;

namespace Nebula.Controllers.Sales;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/sales/{companyId}/[controller]")]
[ApiController]
public class InvoiceSaleController(
    ICompanyService companyService,
    IInvoiceSaleService invoiceSaleService,
    IInvoiceSaleDetailService invoiceSaleDetailService,
    IComprobanteService comprobanteService,
    ICreditNoteService creditNoteService,
    IConsultarValidezComprobanteService consultarValidezComprobanteService,
    IInvoiceHubService invoiceHubService,
    ICreditNoteHubService creditNoteHubService,
    IValidateStockService validateStockService,
    IInvoiceSaleFileService invoiceSaleFileService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery model, [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var comprobantes = await invoiceSaleService.GetComprobantesAsync(companyId, model, page, pageSize);
        var totalProductos = await invoiceSaleService.GetTotalComprobantesAsync(companyId, model);
        var totalPages = (int)Math.Ceiling((double)totalProductos / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<InvoiceSale>
        {
            Pagination = paginationInfo,
            Data = comprobantes
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var responseInvoiceSale = await invoiceSaleService.GetInvoiceSaleAsync(companyId, id);
        return Ok(responseInvoiceSale);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] ComprobanteDto dto)
    {
        var company = await companyService.GetByIdAsync(companyId);
        var comprobante = await comprobanteService.SaveChangesAsync(company, dto);
        await validateStockService.ValidarInvoiceSale(comprobante);
        var invoiceRequest = InvoiceMapper.MapToInvoiceRequestHub(company.Ruc, comprobante);
        var billingResponse = await invoiceHubService.SendInvoiceAsync(companyId, invoiceRequest);
        comprobante.InvoiceSale.BillingResponse = billingResponse;
        await invoiceSaleService.ReplaceOneAsync(comprobante.InvoiceSale.Id, comprobante.InvoiceSale);
        return Ok(new { Data = billingResponse, InvoiceId = comprobante.InvoiceSale.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var invoiceSale = await invoiceSaleService.GetByIdAsync(companyId, id);
        await invoiceSaleService.DeleteOneAsync(companyId, invoiceSale.Id);
        await invoiceSaleDetailService.DeleteOneAsync(companyId, invoiceSale.Id);
        return Ok(invoiceSale);
    }

    [HttpPatch("Reenviar/{id}")]
    public async Task<IActionResult> Reenviar(string companyId, string id)
    {
        var company = await companyService.GetByIdAsync(companyId.Trim());
        var comprobante = new InvoiceSaleAndDetails();
        comprobante.InvoiceSale = await invoiceSaleService.GetByIdAsync(companyId, id);
        comprobante.InvoiceSaleDetails = await invoiceSaleDetailService.GetListAsync(companyId, comprobante.InvoiceSale.Id);
        var invoiceRequest = InvoiceMapper.MapToInvoiceRequestHub(company.Ruc, comprobante);
        var billingResponse = await invoiceHubService.SendInvoiceAsync(companyId, invoiceRequest);
        comprobante.InvoiceSale.BillingResponse = billingResponse;
        await invoiceSaleService.ReplaceOneAsync(comprobante.InvoiceSale.Id, comprobante.InvoiceSale);
        return Ok(new { Data = billingResponse, InvoiceId = comprobante.InvoiceSale.Id });
    }

    [HttpGet("DescargarRegistroVentas")]
    public async Task<IActionResult> DescargarRegistroVentas(string companyId, [FromQuery] DateQuery dto)
    {
        var invoices = await invoiceSaleService.GetMonthlyListAsync(companyId, dto);
        var notes = await creditNoteService.GetListAsync(companyId, dto);
        var datosExcel = new ExcelRegistroVentas(invoices, notes).GenerarArchivo();
        // Configuramos la respuesta HTTP.
        var stream = new MemoryStream();
        datosExcel.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return File(stream, ContentTypeFormat.Excel, "datos.xlsx");
    }

    //[HttpGet("ExcelRegistroVentasF141")]
    //public async Task<IActionResult> ExcelRegistroVentasF141(string companyId, [FromQuery] DateQuery dto)
    //{
    //    string[] fieldNames = new string[] { "Name" };
    //    ParamExcelRegistroVentasF141 param = new ParamExcelRegistroVentasF141
    //    {
    //        InvoiceSeries = await _invoiceSerieService.GetFilteredAsync(companyId, fieldNames, string.Empty),
    //        InvoiceSales = await _invoiceSaleService.GetListAsync(companyId, dto),
    //        CreditNotes = await _creditNoteService.GetListAsync(companyId, dto),
    //    };
    //    // Obtener lista de comprobantes de notas de crédito.
    //    List<string> seriesComprobante = new List<string>();
    //    List<string> númerosComprobante = new List<string>();
    //    param.CreditNotes.ForEach(item =>
    //    {
    //        seriesComprobante.Add(item.NumDocfectado.Split("-")[0].Trim());
    //        númerosComprobante.Add(item.NumDocfectado.Split("-")[1].Trim());
    //    });
    //    seriesComprobante = seriesComprobante.Distinct().ToList();
    //    númerosComprobante = númerosComprobante.Distinct().ToList();
    //    var comprobantesDeNotas = await _invoiceSaleService.GetInvoicesByNumDocs(companyId, seriesComprobante, númerosComprobante);
    //    param.ComprobantesDeNotas = comprobantesDeNotas;
    //    // generar archivo excel y enviar como respuesta de solicitud.
    //    string filePath = new ExcelRegistroVentasF141(param).CrearArchivo();
    //    FileStream stream = new FileStream(filePath, FileMode.Open);
    //    return new FileStreamResult(stream, ContentTypeFormat.Excel);
    //}

    [HttpGet("Pendientes")]
    public async Task<IActionResult> Pendientes(string companyId)
    {
        var pendientes = new List<ComprobantesPendientes>();
        var invoiceSales = await invoiceSaleService.GetInvoiceSalesPendingAsync(companyId);
        invoiceSales.ForEach(item =>
        {
            pendientes.Add(new ComprobantesPendientes()
            {
                ComprobanteId = item.Id,
                TipoDoc = item.TipoDoc,
                Serie = item.Serie,
                Correlativo = item.Correlativo,
                FechaEmision = item.FechaEmision,
                MtoImpVenta = item.MtoImpVenta,
                CdrDescription = item.BillingResponse.CdrDescription,
            });
        });
        var creditNotes = await creditNoteService.GetCreditNotesPendingAsync(companyId);
        creditNotes.ForEach(item =>
        {
            pendientes.Add(new ComprobantesPendientes()
            {
                ComprobanteId = item.Id,
                TipoDoc = item.TipoDoc,
                Serie = item.Serie,
                Correlativo = item.Correlativo,
                FechaEmision = item.FechaEmision,
                MtoImpVenta = item.MtoImpVenta,
                CdrDescription = item.BillingResponse.CdrDescription,
            });
        });

        return Ok(pendientes);
    }

    [HttpPatch("AnularComprobante/{id}")]
    public async Task<IActionResult> AnularComprobante(string companyId, string id)
    {
        var company = await companyService.GetByIdAsync(companyId.Trim());
        var invoiceCancellationResponse = await creditNoteService.InvoiceCancellation(companyId, id);
        var creditNoteRequest = CreditNoteMapper.MapToCreditNoteRequestHub(company.Ruc, invoiceCancellationResponse);
        var billingResponse = await creditNoteHubService.SendCreditNoteAsync(companyId, creditNoteRequest);
        var creditNote = invoiceCancellationResponse.CreditNote;
        creditNote.BillingResponse = billingResponse;
        await creditNoteService.ReplaceOneAsync(creditNote.Id, creditNote);
        var invoice = invoiceCancellationResponse.InvoiceSale;
        invoice.Anulada = billingResponse.Success;
        await invoiceSaleService.ReplaceOneAsync(invoice.Id, invoice);
        return Ok(new { billingResponse, creditNote });
    }

    [HttpGet("Ticket/{id}")]
    public async Task<IActionResult> Ticket(string companyId, string id)
    {
        var responseInvoice = await invoiceSaleService.GetInvoiceSaleAsync(companyId, id);
        var ticket = new TicketDto()
        {
            Company = await companyService.GetByIdAsync(companyId.Trim()),
            InvoiceSale = responseInvoice.InvoiceSale,
            InvoiceSaleDetails = responseInvoice.InvoiceSaleDetails,
        };
        return Ok(ticket);
    }

    [AllowAnonymous]
    [HttpGet("ConsultarValidez")]
    public async Task<IActionResult> ConsultarValidez(string companyId,
        [FromQuery] QueryConsultarValidezComprobante query)
    {
        string pathArchivoZip = await consultarValidezComprobanteService.CrearArchivosDeValidación(new Company(), query);
        FileStream stream = new FileStream(pathArchivoZip, FileMode.Open);
        return new FileStreamResult(stream, "application/zip");
    }

    [HttpGet("GetXml/{invoiceId}")]
    public async Task<IActionResult> GetXml(string companyId, string invoiceId)
    {
        var company = await companyService.GetByIdAsync(companyId);
        var invoice = await invoiceSaleService.GetByIdAsync(companyId, invoiceId);
        var xml = invoiceSaleFileService.GetXml(company, invoice);
        return new FileStreamResult(xml, "application/xml");
    }
}
