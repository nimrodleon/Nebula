using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Dto.Sales;
using Nebula.Database.Helpers;
using Nebula.Database.Services.Common;
using Nebula.Database.Services.Sales;

namespace Nebula.Controllers.Sales;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class CreditNoteController : ControllerBase
{
    private readonly ConfigurationService _configurationService;
    private readonly CreditNoteService _creditNoteService;

    public CreditNoteController(ConfigurationService configurationService, CreditNoteService creditNoteService)
    {
        _configurationService = configurationService;
        _creditNoteService = creditNoteService;
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var creditNote = await _creditNoteService.GetCreditNoteByInvoiceSaleIdAsync(id);
        return Ok(creditNote);
    }

    [HttpPatch("SituacionFacturador/{id}")]
    public async Task<IActionResult> SituacionFacturador(string id, [FromBody] SituacionFacturadorDto dto)
    {
        var creditNote = await _creditNoteService.SetSituacionFacturador(id, dto);
        return Ok(creditNote);
    }

    [AllowAnonymous]
    [HttpGet("GetPdf/{id}")]
    public async Task<IActionResult> GetPdf(string id)
    {
        var configuration = await _configurationService.GetAsync();
        var creditNote = await _creditNoteService.GetByIdAsync(id);
        // 20520485750-07-BC01-00000008
        string nomArch = $"{configuration.Ruc}-07-{creditNote.Serie}-{creditNote.Number}.pdf";
        string sfs = Path.Combine(configuration.FileSunat, "sfs");
        string pdfFolder = Path.Combine(sfs, "REPO");
        string pathPdf = Path.Combine(pdfFolder, nomArch);
        FileStream stream = new FileStream(pathPdf, FileMode.Open);
        return new FileStreamResult(stream, "application/pdf");
    }
}
