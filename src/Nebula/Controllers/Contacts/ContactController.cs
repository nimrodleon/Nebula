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
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/contacts/[controller]")]
[ApiController]
public class ContactController(
    IUserAuthenticationService userAuthenticationService,
    IContactService contactService,
    ICashierDetailService cashierDetailService,
    IInvoiceSaleService invoiceSaleService,
    IContribuyenteService contribuyenteService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var contacts = await contactService.GetContactosAsync(_companyId, query, page, pageSize);
        var totalContacts = await contactService.GetTotalContactosAsync(_companyId, query);
        var totalPages = (int)Math.Ceiling((double)totalContacts / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<Contact>
        {
            Pagination = paginationInfo,
            Data = contacts
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var contact = await contactService.GetByIdAsync(_companyId, id);
        return Ok(contact);
    }

    [Obsolete]
    [HttpGet("Document/{document}")]
    public async Task<IActionResult> Document(string document)
    {
        var contact = await contactService.GetContactByDocumentAsync(_companyId, document);
        return Ok(contact);
    }

    [HttpGet("Contribuyente/{doc}")]
    public IActionResult Contribuyente(string doc)
    {
        ContribuyenteDto? result = null;
        if (doc.Trim().Length == 11)
            result = contribuyenteService.GetByRuc(doc.Trim());
        if (doc.Trim().Length == 8)
            result = contribuyenteService.GetByDni(doc.Trim());
        if (result == null) return BadRequest();
        return Ok(result);
    }

    [HttpGet("Select2")]
    public async Task<IActionResult> Select2([FromQuery] string? term)
    {
        if (term == null) term = string.Empty;
        var fieldNames = new string[] { "Document", "Name" };
        var responseData = await contactService.GetFilteredAsync(_companyId, fieldNames, term, 6);
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
    public async Task<IActionResult> Create([FromBody] Contact model)
    {
        try
        {
            model.CompanyId = _companyId.Trim();
            model.Document = model.Document.Trim();
            model.Name = model.Name.Trim().ToUpper();
            model.Address = model.Address.Trim().ToUpper();
            model.CodUbigeo = model.CodUbigeo.Trim();
            await contactService.InsertOneAsync(model);
            return Ok(model);
        }
        catch (MongoWriteException ex)
            when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un contacto con el mismo número de documento para esta empresa.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Contact model)
    {
        try
        {
            var contact = await contactService.GetByIdAsync(_companyId, id);
            model.Id = contact.Id;
            model.CompanyId = _companyId.Trim();
            model.Document = model.Document.Trim();
            model.Name = model.Name.Trim().ToUpper();
            model.Address = model.Address.Trim().ToUpper();
            model.CodUbigeo = model.CodUbigeo.Trim();
            contact = await contactService.ReplaceOneAsync(contact.Id, model);
            return Ok(contact);
        }
        catch (MongoWriteException ex)
            when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return BadRequest("Ya existe un contacto con el mismo número de documento para esta empresa.");
        }
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var contact = await contactService.GetByIdAsync(_companyId, id);
        await contactService.DeleteOneAsync(_companyId, id);
        return Ok(contact);
    }

    [HttpGet("EntradaSalida/{id}")]
    public async Task<IActionResult> EntradaSalida(string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await cashierDetailService.GetEntradaSalidaAsync(_companyId, id, month, year);
        return Ok(responseData);
    }

    [HttpGet("InvoiceSale/{id}")]
    public async Task<IActionResult> InvoiceSale(string id, [FromQuery] string month, [FromQuery] string year)
    {
        var responseData = await invoiceSaleService.GetByContactIdAsync(_companyId, id, month, year);
        return Ok(responseData);
    }
}
