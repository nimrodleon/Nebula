using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Cashier;
using Nebula.Modules.Contacts.Models;
using Nebula.Modules.Contacts;
using Nebula.Modules.Contacts.Dto;
using Nebula.Modules.Sales.Invoices;
using Microsoft.AspNetCore.Authorization;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using MongoDB.Driver;
using Nebula.Common.Helpers;

namespace Nebula.Controllers.Contacts;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/contacts/{companyId}/[controller]")]
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
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var contacts = await _contactService.GetContactosAsync(companyId, query, page, pageSize);
        var totalContacts = await _contactService.GetTotalContactosAsync(companyId, query);
        var totalPages = (int)Math.Ceiling((double)totalContacts / pageSize);
        string urlController = $"api/contacts/{companyId}/Contact";

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks(maxVisiblePages: 6, urlController);

        var result = new PaginationResult<Contact>
        {
            Pagination = paginationInfo,
            Data = contacts
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var contact = await _contactService.GetByIdAsync(companyId, id);
        return Ok(contact);
    }

    [Obsolete]
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
        var responseData = await _contactService.GetContactosAsync(companyId, term, 6);
        var data = new List<ContactSelect>();
        responseData.ForEach(item =>
        {
            data.Add(new ContactSelect()
            {
                Id = item.Id,
                Text = $"{item.Document} - {item.Name}",
                CompanyId = item.CompanyId,
                Document = item.Document,
                DocType = item.DocType,
                Name = item.Name,
                Address = item.Address,
                PhoneNumber = item.PhoneNumber,
                CodUbigeo = item.CodUbigeo,
            });
        });
        return Ok(new { Results = data });
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] Contact model)
    {
        try
        {
            model.CompanyId = companyId.Trim();
            model.Document = model.Document.Trim();
            model.Name = model.Name.Trim().ToUpper();
            model.Address = model.Address.Trim().ToUpper();
            model.CodUbigeo = model.CodUbigeo.Trim();
            await _contactService.CreateAsync(model);
            return Ok(model);
        }
        catch (MongoWriteException ex)
        when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un contacto con el mismo número de documento para esta empresa.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] Contact model)
    {
        try
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
        catch (MongoWriteException ex)
        when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un contacto con el mismo número de documento para esta empresa.");
        }
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = CompanyRoles.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var contact = await _contactService.GetByIdAsync(companyId, id);
        await _contactService.RemoveAsync(companyId, id);
        return Ok(contact);
    }

    [HttpGet("EntradaSalida/{id}")]
    public async Task<IActionResult> EntradaSalida(string companyId, string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await _cashierDetailService.GetEntradaSalidaAsync(companyId, id, month, year);
        return Ok(responseData);
    }

    [HttpGet("InvoiceSale/{id}")]
    public async Task<IActionResult> InvoiceSale(string companyId, string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await _invoiceSaleService.GetByContactIdAsync(companyId, id, month, year);
        return Ok(responseData);
    }
}
