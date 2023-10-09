using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Sales.Notes;
using Nebula.Modules.Sales.Notes.Dto;
using Nebula.Common;

namespace Nebula.Controllers.Sales;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/sales/{companyId}/[controller]")]
[ApiController]
public class CreditNoteController : ControllerBase
{
    private readonly ICacheAuthService _cacheAuthService;
    private readonly ICreditNoteService _creditNoteService;

    public CreditNoteController(ICacheAuthService cacheAuthService, ICreditNoteService creditNoteService)
    {
        _cacheAuthService = cacheAuthService;
        _creditNoteService = creditNoteService;
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
            TributosCreditNote = creditNoteDto.TributosCreditNote,
        };
        return Ok(printCreditNote);
    }

}
