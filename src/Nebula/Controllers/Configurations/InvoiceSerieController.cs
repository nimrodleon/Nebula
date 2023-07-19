using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Configurations.Warehouses;

namespace Nebula.Controllers.Configurations;

[Route("api/[controller]")]
[ApiController]
public class InvoiceSerieController : ControllerBase
{
    private readonly IInvoiceSerieService _invoiceSerieService;

    public InvoiceSerieController(IInvoiceSerieService invoiceSerieService) =>
        _invoiceSerieService = invoiceSerieService;

    [HttpGet("Index"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _invoiceSerieService.GetAsync("Name", query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.ConfigurationRead)]
    public async Task<IActionResult> Show(string id)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(id);
        return Ok(invoiceSerie);
    }

    [HttpPost("Create"), UserAuthorize(Permission.ConfigurationCreate)]
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

    [HttpPut("Update/{id}"), UserAuthorize(Permission.ConfigurationEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] InvoiceSerie model)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(id);

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

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.ConfigurationDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(id);
        await _invoiceSerieService.RemoveAsync(id);
        return Ok(new { Ok = true, Data = invoiceSerie, Msg = "La serie de facturación ha sido borrado!" });
    }
}
