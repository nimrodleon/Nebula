using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Configurations.Models;

namespace Nebula.Controllers.Common;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class InvoiceSerieController : ControllerBase
{
    private readonly CrudOperationService<InvoiceSerie> _invoiceSerieService;

    public InvoiceSerieController(CrudOperationService<InvoiceSerie> invoiceSerieService) =>
        _invoiceSerieService = invoiceSerieService;

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _invoiceSerieService.GetAsync("Name", query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(id);
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

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(id);
        await _invoiceSerieService.RemoveAsync(id);
        return Ok(new { Ok = true, Data = invoiceSerie, Msg = "La serie de facturación ha sido borrado!" });
    }
}
