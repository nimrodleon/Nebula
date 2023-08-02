using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Cashier;
using Nebula.Modules.Contacts.Models;
using Nebula.Modules.Contacts;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Contacts.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Contacts;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ICashierDetailService _cashierDetailService;
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IContribuyenteService _contribuyenteService;

    public ContactController(IContactService contactService,
        ICashierDetailService cashierDetailService,
        IInvoiceSaleService invoiceSaleService,
        IContribuyenteService contribuyenteService)
    {
        _contactService = contactService;
        _cashierDetailService = cashierDetailService;
        _invoiceSaleService = invoiceSaleService;
        _contribuyenteService = contribuyenteService;
    }

    [HttpGet("Index"), UserAuthorize(Permission.ContactRead)]
    public async Task<IActionResult> Index([FromQuery] string? query)
    {
        if (string.IsNullOrEmpty(query)) query = string.Empty;
        var contacts = await _contactService.GetContactsAsync(query);
        return Ok(contacts);
    }

    [HttpGet("Show/{id}"), UserAuthorize(Permission.ContactRead)]
    public async Task<IActionResult> Show(string id)
    {
        var contact = await _contactService.GetByIdAsync(id);
        return Ok(contact);
    }

    [HttpGet("Document/{document}"), UserAuthorize(Permission.ContactRead)]
    public async Task<IActionResult> Document(string document)
    {
        var contact = await _contactService.GetContactByDocumentAsync(document);
        return Ok(contact);
    }

    [HttpGet("Contribuyente/{doc}"), UserAuthorize(Permission.ContactRead)]
    public IActionResult Contribuyente(string doc)
    {
        ContribuyenteDto? result = null;
        if (doc.Trim().Length == 11)
            result = _contribuyenteService.GetByRuc(doc.Trim());
        if (doc.Trim().Length == 8)
            result = _contribuyenteService.GetByDni(doc.Trim());
        if (result == null) return BadRequest();
        return Ok(result);
    }

    [HttpGet("Select2"), UserAuthorize(Permission.ContactRead)]
    public async Task<IActionResult> Select2([FromQuery] string? term)
    {
        if (term == null) term = string.Empty;
        var responseData = await _contactService.GetContactsAsync(term, 10);
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

    [HttpPost("Create"), UserAuthorize(Permission.ContactCreate)]
    public async Task<IActionResult> Create([FromBody] Contact model)
    {
        model.Document = model.Document.Trim();
        model.Name = model.Name.Trim().ToUpper();
        model.Address = model.Address.Trim().ToUpper();
        model.CodUbigeo = model.CodUbigeo.Trim();
        await _contactService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.ContactEdit)]
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

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.ContactDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var contact = await _contactService.GetByIdAsync(id);
        await _contactService.RemoveAsync(id);
        return Ok(contact);
    }

    [HttpGet("EntradaSalida/{id}"), UserAuthorize(Permission.ContactRead)]
    public async Task<IActionResult> EntradaSalida(string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await _cashierDetailService.GetEntradaSalidaAsync(id, month, year);
        return Ok(responseData);
    }

    [HttpGet("InvoiceSale/{id}"), UserAuthorize(Permission.ContactRead)]
    public async Task<IActionResult> InvoiceSale(string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await _invoiceSaleService.GetByContactIdAsync(id, month, year);
        return Ok(responseData);
    }
}
