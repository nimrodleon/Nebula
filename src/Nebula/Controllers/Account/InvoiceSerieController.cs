using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Account;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/account/{companyId}/[controller]")]
[ApiController]
public class InvoiceSerieController : ControllerBase
{
    private readonly IInvoiceSerieService _invoiceSerieService;

    public InvoiceSerieController(IInvoiceSerieService invoiceSerieService) =>
        _invoiceSerieService = invoiceSerieService;

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Name" };
        var invoiceSeries = await _invoiceSerieService.GetFilteredAsync(companyId, fieldNames, query);
        return Ok(invoiceSeries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, id);
        return Ok(invoiceSerie);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] InvoiceSerie model)
    {
        model.CompanyId = companyId.Trim();
        model.Name = model.Name.ToUpper();
        await _invoiceSerieService.CreateAsync(model);
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] InvoiceSerie model)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, id);
        model.Id = invoiceSerie.Id;
        model.CompanyId = companyId.Trim();
        model.Name = model.Name.ToUpper();
        model = await _invoiceSerieService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, id);
        await _invoiceSerieService.RemoveAsync(companyId, invoiceSerie.Id);
        return Ok(invoiceSerie);
    }
}
