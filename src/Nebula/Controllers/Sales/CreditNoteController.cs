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
[Route("api/sales/{companyId}/[controller]")]
[ApiController]
public class CreditNoteController(
    ICompanyService companyService,
    IInvoiceSaleService invoiceSaleService,
    ICreditNoteService creditNoteService,
    ICreditNoteDetailService creditNoteDetailService,
    ICreditNoteHubService creditNoteHubService)
    : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var creditNote = await creditNoteService.GetCreditNoteByInvoiceSaleIdAsync(companyId, id);
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
        var company = await companyService.GetByIdAsync(companyId.Trim());
        var creditNoteDto = await creditNoteService.GetCreditNoteDtoAsync(companyId, creditNoteId);
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
        var company = await companyService.GetByIdAsync(companyId.Trim());
        var cancellationResponse = new InvoiceCancellationResponse();
        cancellationResponse.CreditNote = await creditNoteService.GetByIdAsync(companyId, creditNoteId);
        cancellationResponse.CreditNoteDetail = await creditNoteDetailService.GetListAsync(companyId, creditNoteId);
        cancellationResponse.InvoiceSale = await invoiceSaleService.GetByIdAsync(companyId, cancellationResponse.CreditNote.InvoiceSaleId);
        var creditNoteRequest = CreditNoteMapper.MapToCreditNoteRequestHub(company.Ruc, cancellationResponse);
        var billingResponse = await creditNoteHubService.SendCreditNoteAsync(companyId, creditNoteRequest);
        var creditNote = cancellationResponse.CreditNote;
        creditNote.BillingResponse = billingResponse;
        await creditNoteService.ReplaceOneAsync(creditNote.Id, creditNote);
        var invoice = cancellationResponse.InvoiceSale;
        invoice.Anulada = billingResponse.Success;
        await invoiceSaleService.ReplaceOneAsync(invoice.Id, invoice);
        return Ok(new { billingResponse, creditNote });
    }

}
