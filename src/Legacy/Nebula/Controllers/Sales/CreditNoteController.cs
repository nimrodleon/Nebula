using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Sales.Notes;
using Nebula.Modules.Sales.Notes.Dto;
using Nebula.Common;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.InvoiceHub.Helpers;
using Nebula.Modules.InvoiceHub;

namespace Nebula.Controllers.Sales;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/sales/{companyId}/[controller]")]
[ApiController]
public class CreditNoteController : ControllerBase
{
    private readonly ICacheAuthService _cacheAuthService;
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly ICreditNoteService _creditNoteService;
    private readonly ICreditNoteDetailService _creditNoteDetailService;
    private readonly ICreditNoteHubService _creditNoteHubService;

    public CreditNoteController(ICacheAuthService cacheAuthService,
        IInvoiceSaleService invoiceSaleService, ICreditNoteService creditNoteService,
        ICreditNoteDetailService creditNoteDetailService, ICreditNoteHubService creditNoteHubService)
    {
        _cacheAuthService = cacheAuthService;
        _invoiceSaleService = invoiceSaleService;
        _creditNoteService = creditNoteService;
        _creditNoteDetailService = creditNoteDetailService;
        _creditNoteHubService = creditNoteHubService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var creditNote = await _creditNoteService.GetCreditNoteByInvoiceSaleIdAsync(companyId, id);
        return Ok(creditNote);
    }

    /// <summary>
    /// Datos de Impresión Nota de Crédito.
    /// </summary>
    /// <param name="creditNoteId">Identificador de la Nota de crédito</param>
    /// <returns>JSON[PrintCreditNoteDto]</returns>
    [HttpGet("Print/{creditNoteId}")]
    public async Task<IActionResult> Print(string companyId, string creditNoteId)
    {
        var company = await _cacheAuthService.GetCompanyByIdAsync(companyId);
        var creditNoteDto = await _creditNoteService.GetCreditNoteDtoAsync(companyId, creditNoteId);
        var printCreditNote = new PrintCreditNoteDto()
        {
            Company = company,
            CreditNote = creditNoteDto.CreditNote,
            CreditNoteDetails = creditNoteDto.CreditNoteDetails,
        };
        return Ok(printCreditNote);
    }

    [HttpPatch("Reenviar/{creditNoteId}")]
    public async Task<IActionResult> Reenviar(string companyId, string creditNoteId)
    {
        var company = await _cacheAuthService.GetCompanyByIdAsync(companyId);
        var cancellationResponse = new InvoiceCancellationResponse();
        cancellationResponse.CreditNote = await _creditNoteService.GetByIdAsync(companyId, creditNoteId);
        cancellationResponse.CreditNoteDetail = await _creditNoteDetailService.GetListAsync(companyId, creditNoteId);
        cancellationResponse.InvoiceSale = await _invoiceSaleService.GetByIdAsync(companyId, cancellationResponse.CreditNote.InvoiceSaleId);
        var creditNoteRequest = CreditNoteMapper.MapToCreditNoteRequestHub(company.Ruc, cancellationResponse);
        var billingResponse = await _creditNoteHubService.SendCreditNoteAsync(companyId, creditNoteRequest);
        var creditNote = cancellationResponse.CreditNote;
        creditNote.BillingResponse = billingResponse;
        await _creditNoteService.UpdateAsync(creditNote.Id, creditNote);
        var invoice = cancellationResponse.InvoiceSale;
        invoice.Anulada = billingResponse.Success;
        await _invoiceSaleService.UpdateAsync(invoice.Id, invoice);
        return Ok(new { billingResponse, creditNote });
    }

}
