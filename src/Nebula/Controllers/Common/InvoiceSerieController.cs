using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Models.Common;
using Nebula.Data.Services;
using Nebula.Data.Services.Common;

namespace Nebula.Controllers.Common;

[Route("api/[controller]")]
[ApiController]
public class InvoiceSerieController : ControllerBase
{
    private readonly InvoiceSerieService _invoiceSerieService;

    public InvoiceSerieController(InvoiceSerieService invoiceSerieService) =>
        _invoiceSerieService = invoiceSerieService;

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _invoiceSerieService.GetListAsync(query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var invoiceSerie = await _invoiceSerieService.GetAsync(id);
        if (invoiceSerie is null) return NotFound();
        return Ok(invoiceSerie);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] InvoiceSerie model)
    {
        model.Name = model.Name.ToUpper();
        await _invoiceSerieService.CreateAsync(model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"La serie {model.Name} ha sido registrado!"
        });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] InvoiceSerie model)
    {
        var invoiceSerie = await _invoiceSerieService.GetAsync(id);
        if (invoiceSerie is null) return NotFound();

        model.Id = invoiceSerie.Id;
        model.Name = model.Name.ToUpper();
        await _invoiceSerieService.UpdateAsync(id, model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"La serie {model.Name} ha sido actualizado!"
        });
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var invoiceSerie = await _invoiceSerieService.GetAsync(id);
        if (invoiceSerie is null) return NotFound();
        await _invoiceSerieService.RemoveAsync(id);
        return Ok(new { Ok = true, Data = invoiceSerie, Msg = "La serie de facturación ha sido borrado!" });
    }
}
