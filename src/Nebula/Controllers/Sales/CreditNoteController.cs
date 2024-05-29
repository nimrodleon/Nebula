using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Sales.Notes;
using Nebula.Modules.Sales.Notes.Dto;
using Nebula.Modules.Account;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.InvoiceHub.Helpers;
using Nebula.Modules.InvoiceHub;

namespace Nebula.Controllers.Sales;

[Authorize]
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/sales/[controller]")]
[ApiController]
public class CreditNoteController(
    IUserAuthenticationService userAuthenticationService,
    ICompanyService companyService,
    IInvoiceSaleService invoiceSaleService,
    ICreditNoteService creditNoteService,
    ICreditNoteDetailService creditNoteDetailService,
    ICreditNoteHubService creditNoteHubService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var creditNote = await creditNoteService.GetCreditNoteByInvoiceSaleIdAsync(_companyId, id);
        return Ok(creditNote);
    }

    /// <summary>
    /// Datos de Impresión Nota de Crédito.
    /// </summary>
    /// <param name="creditNoteId">Identificador de la Nota de crédito</param>
    /// <returns>JSON[PrintCreditNoteDto]</returns>
    [HttpGet("Print/{creditNoteId}")]
    public async Task<IActionResult> Print(string creditNoteId)
    {
        var company = await companyService.GetByIdAsync(_companyId.Trim());
        var creditNoteDto = await creditNoteService.GetCreditNoteDtoAsync(_companyId, creditNoteId);
        var printCreditNote = new PrintCreditNoteDto()
        {
            Company = company,
            CreditNote = creditNoteDto.CreditNote,
            CreditNoteDetails = creditNoteDto.CreditNoteDetails,
        };
        return Ok(printCreditNote);
    }

    [HttpPatch("Reenviar/{creditNoteId}")]
    public async Task<IActionResult> Reenviar(string creditNoteId)
    {
        var company = await companyService.GetByIdAsync(_companyId.Trim());
        var cancellationResponse = new InvoiceCancellationResponse();
        cancellationResponse.CreditNote = await creditNoteService.GetByIdAsync(_companyId, creditNoteId);
        cancellationResponse.CreditNoteDetail = await creditNoteDetailService.GetListAsync(_companyId, creditNoteId);
        cancellationResponse.InvoiceSale =
            await invoiceSaleService.GetByIdAsync(_companyId, cancellationResponse.CreditNote.InvoiceSaleId);
        var creditNoteRequest = CreditNoteMapper.MapToCreditNoteRequestHub(company.Ruc, cancellationResponse);
        var billingResponse = await creditNoteHubService.SendCreditNoteAsync(_companyId, creditNoteRequest);
        var creditNote = cancellationResponse.CreditNote;
        creditNote.BillingResponse = billingResponse;
        await creditNoteService.ReplaceOneAsync(creditNote.Id, creditNote);
        var invoice = cancellationResponse.InvoiceSale;
        invoice.Anulada = billingResponse.Success;
        await invoiceSaleService.ReplaceOneAsync(invoice.Id, invoice);
        return Ok(new { billingResponse, creditNote });
    }
}
