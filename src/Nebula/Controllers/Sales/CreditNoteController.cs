using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Sales;
using Nebula.Modules.Sales.Notes;

namespace Nebula.Controllers.Sales;

[Route("api/sales/{companyId}/[controller]")]
[ApiController]
public class CreditNoteController : ControllerBase
{
    private readonly ICreditNoteService _creditNoteService;

    public CreditNoteController(ICreditNoteService creditNoteService)
    {
        _creditNoteService = creditNoteService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var creditNote = await _creditNoteService.GetCreditNoteByInvoiceSaleIdAsync(companyId, id);
        return Ok(creditNote);
    }

    [HttpPatch("SituacionFacturador/{id}")]
    public async Task<IActionResult> SituacionFacturador(string companyId, string id, [FromBody] SituacionFacturadorDto dto)
    {
        var creditNote = await _creditNoteService.SetSituacionFacturador(companyId, id, dto);
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
        var printCreditNoteDto = await _creditNoteService.GetPrintCreditNoteDto(companyId, creditNoteId);
        return Ok(printCreditNoteDto);
    }

}
