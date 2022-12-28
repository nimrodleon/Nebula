using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Services.Sales;

namespace Nebula.Controllers.Sales;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class CreditNoteController : ControllerBase
{
    private readonly CreditNoteService _creditNoteService;

    public CreditNoteController(CreditNoteService creditNoteService)
    {
        _creditNoteService = creditNoteService;
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var creditNote = await _creditNoteService.GetCreditNoteByInvoiceSaleIdAsync(id);
        return Ok(creditNote);
    }
}
