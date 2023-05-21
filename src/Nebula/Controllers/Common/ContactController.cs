using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Common;
using Nebula.Database.Dto.Common;
using Nebula.Database.Services.Cashier;
using Nebula.Database.Services.Common;
using Nebula.Modules.Sales;

namespace Nebula.Controllers.Common;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly ContactService _contactService;
    private readonly CashierDetailService _cashierDetailService;
    private readonly InvoiceSaleService _invoiceSaleService;

    public ContactController(ContactService contactService,
        CashierDetailService cashierDetailService, InvoiceSaleService invoiceSaleService)
    {
        _contactService = contactService;
        _cashierDetailService = cashierDetailService;
        _invoiceSaleService = invoiceSaleService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        if (string.IsNullOrEmpty(query)) query = string.Empty;
        var contacts = await _contactService.GetContactsAsync(query);
        return Ok(contacts);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var contact = await _contactService.GetByIdAsync(id);
        return Ok(contact);
    }

    [HttpGet("Document/{document}")]
    public async Task<IActionResult> Document(string document)
    {
        var contact = await _contactService.GetContactByDocumentAsync(document);
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
                Name = item.Name,
                Address = item.Address,
                CodUbigeo = item.CodUbigeo,
            });
        });
        return Ok(new { Results = data });
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] Contact model)
    {
        model.Document = model.Document.Trim();
        model.Name = model.Name.Trim().ToUpper();
        model.Address = model.Address.Trim().ToUpper();
        model.CodUbigeo = model.CodUbigeo.Trim();
        await _contactService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Contact model)
    {
        var contact = await _contactService.GetByIdAsync(id);
        model.Id = contact.Id;
        model.Document = model.Document.Trim();
        model.Name = model.Name.Trim().ToUpper();
        model.Address = model.Address.Trim().ToUpper();
        model.CodUbigeo = model.CodUbigeo.Trim();
        contact = await _contactService.UpdateAsync(contact.Id, model);
        return Ok(contact);
    }

    [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var contact = await _contactService.GetByIdAsync(id);
        await _contactService.RemoveAsync(id);
        return Ok(contact);
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
