using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Cashier;
using Nebula.Modules.Contacts.Models;
using Nebula.Modules.Contacts;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Contacts.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Nebula.Controllers.Contacts;

[Authorize]
[Route("api/{companyId}/[controller]")]
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

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        var contacts = await _contactService.GetContactsAsync(companyId, query);
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var contact = await _contactService.GetByIdAsync(companyId, id);
        return Ok(contact);
    }

    [HttpGet("Document/{document}")]
    public async Task<IActionResult> Document(string companyId, string document)
    {
        var contact = await _contactService.GetContactByDocumentAsync(companyId, document);
        return Ok(contact);
    }

    [HttpGet("Contribuyente/{doc}")]
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

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2(string companyId, [FromQuery] string? term)
    {
        if (term == null) term = string.Empty;
        var responseData = await _contactService.GetContactsAsync(companyId, term, 10);
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

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Contact model)
    {
        model.CompanyId = companyId.Trim();
        model.Document = model.Document.Trim();
        model.Name = model.Name.Trim().ToUpper();
        model.Address = model.Address.Trim().ToUpper();
        model.CodUbigeo = model.CodUbigeo.Trim();
        await _contactService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Contact model)
    {
        var contact = await _contactService.GetByIdAsync(companyId, id);
        model.Id = contact.Id;
        model.CompanyId = companyId.Trim();
        model.Document = model.Document.Trim();
        model.Name = model.Name.Trim().ToUpper();
        model.Address = model.Address.Trim().ToUpper();
        model.CodUbigeo = model.CodUbigeo.Trim();
        contact = await _contactService.UpdateAsync(contact.Id, model);
        return Ok(contact);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var contact = await _contactService.GetByIdAsync(companyId, id);
        await _contactService.RemoveAsync(companyId, id);
        return Ok(contact);
    }

    [HttpGet("EntradaSalida/{id}")]
    public async Task<IActionResult> EntradaSalida(string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await _cashierDetailService.GetEntradaSalidaAsync(id, month, year);
        return Ok(responseData);
    }

    [HttpGet("InvoiceSale/{id}")]
    public async Task<IActionResult> InvoiceSale(string companyId, string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await _invoiceSaleService.GetByContactIdAsync(companyId, id, month, year);
        return Ok(responseData);
    }
}
