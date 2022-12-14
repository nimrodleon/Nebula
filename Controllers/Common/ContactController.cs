using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Common;
using Nebula.Database.Dto.Common;
using Nebula.Database.Services;
using Nebula.Database.Services.Cashier;
using Nebula.Database.Services.Sales;

namespace Nebula.Controllers.Common;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly CrudOperationService<Contact> _contactService;
    private readonly CashierDetailService _cashierDetailService;
    private readonly InvoiceSaleService _invoiceSaleService;

    public ContactController(CrudOperationService<Contact> contactService,
        CashierDetailService cashierDetailService, InvoiceSaleService invoiceSaleService)
    {
        _contactService = contactService;
        _cashierDetailService = cashierDetailService;
        _invoiceSaleService = invoiceSaleService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        var responseData = await _contactService.GetAsync("Name", query);
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var contact = await _contactService.GetAsync(id);
        return Ok(contact);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2([FromQuery] string? term)
    {
        var responseData = await _contactService.GetAsync("Name", term, 10);
        var data = new List<ContactSelect>();
        responseData.ForEach(item =>
        {
            data.Add(new ContactSelect()
            {
                Id = item.Id,
                Text = $"{item.Document} - {item.Name}",
                DocType = item.DocType,
                Document = item.Document,
                Name = item.Name
            });
        });
        return Ok(new { Results = data });
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Contact model)
    {
        model.Name = model.Name.ToUpper();
        await _contactService.CreateAsync(model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"El contacto {model.Name} ha sido registrado!"
        });
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Contact model)
    {
        var contact = await _contactService.GetAsync(id);
        model.Id = contact.Id;
        model.Name = model.Name.ToUpper();
        await _contactService.UpdateAsync(id, model);

        return Ok(new
        {
            Ok = true,
            Data = model,
            Msg = $"El contacto {model.Name} ha sido actualizado!"
        });
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var contact = await _contactService.GetAsync(id);
        await _contactService.RemoveAsync(id);
        return Ok(new { Ok = true, Data = contact, Msg = "El contacto ha sido borrado!" });
    }

    [HttpGet("EntradaSalida/{id}")]
    public async Task<IActionResult> EntradaSalida(string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await _cashierDetailService.GetEntradaSalidaAsync(id, month, year);
        return Ok(responseData);
    }

    [HttpGet("InvoiceSale/{id}")]
    public async Task<IActionResult> InvoiceSale(string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await _invoiceSaleService.GetByContactIdAsync(id, month, year);
        return Ok(responseData);
    }
}
