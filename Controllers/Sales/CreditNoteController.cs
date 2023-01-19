using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Dto.Sales;
using Nebula.Database.Helpers;
using Nebula.Database.Services.Common;
using Nebula.Database.Services.Facturador;
using Nebula.Database.Services.Sales;

namespace Nebula.Controllers.Sales;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class CreditNoteController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ConfigurationService _configurationService;
    private readonly CreditNoteService _creditNoteService;
    private readonly FacturadorService _facturadorService;

    public CreditNoteController(IConfiguration configuration, ConfigurationService configurationService,
        CreditNoteService creditNoteService, FacturadorService facturadorService)
    {
        _configuration = configuration;
        _configurationService = configurationService;
        _creditNoteService = creditNoteService;
        _facturadorService = facturadorService;
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

    [HttpPatch("SaveInControlFolder/{creditNoteId}")]
    public async Task<IActionResult> SituacionFacturador(string creditNoteId)
    {
        try
        {
            var creditNote = await _facturadorService.SaveCreditNoteInControlFolder(creditNoteId);
            return Ok(creditNote);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("GetPdf/{id}")]
    public async Task<IActionResult> GetPdf(string id)
    {
        var configuration = await _configurationService.GetAsync();
        var creditNote = await _creditNoteService.GetByIdAsync(id);
        // 20520485750-07-BC01-00000008
        string nomArch = $"{configuration.Ruc}-07-{creditNote.Serie}-{creditNote.Number}.pdf";
        string pathPdf = string.Empty;
        var storagePath = _configuration.GetValue<string>("StoragePath");
        if (creditNote.DocumentPath == DocumentPathType.SFS)
        {
            string carpetaArchivoSunat = Path.Combine(configuration.SunatArchivos, "sfs");
            pathPdf = Path.Combine(carpetaArchivoSunat, "REPO", nomArch);
        }
        if (creditNote.DocumentPath == DocumentPathType.CONTROL)
        {
            string carpetaArchivoSunat = Path.Combine(storagePath, "sunat");
            string carpetaRepo = Path.Combine(carpetaArchivoSunat, "REPO", creditNote.Year, creditNote.Month);
            pathPdf = Path.Combine(carpetaRepo, nomArch);
        }
        FileStream stream = new FileStream(pathPdf, FileMode.Open);
        return new FileStreamResult(stream, "application/pdf");
    }
}
