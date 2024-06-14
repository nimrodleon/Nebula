using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Account;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth;

namespace Nebula.Controllers.Account;

[Authorize]
[Route("api/account/[controller]")]
[ApiController]
public class InvoiceSerieController(
    IUserAuthenticationService userAuthenticationService,
    IInvoiceSerieService invoiceSerieService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string query = "")
    {
        string[] fieldNames = new string[] { "Name" };
        var invoiceSeries = await invoiceSerieService.GetFilteredAsync(_companyId, fieldNames, query);
        return Ok(invoiceSeries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var invoiceSerie = await invoiceSerieService.GetByIdAsync(_companyId, id);
        return Ok(invoiceSerie);
    }

    [BusinessAuthorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InvoiceSerie model)
    {
        model.CompanyId = _companyId.Trim();
        model.Name = model.Name.ToUpper();
        await invoiceSerieService.InsertOneAsync(model);
        return Ok(model);
    }

    [BusinessAuthorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] InvoiceSerie model)
    {
        var invoiceSerie = await invoiceSerieService.GetByIdAsync(_companyId, id);
        model.Id = invoiceSerie.Id;
        model.CompanyId = _companyId.Trim();
        model.Name = model.Name.ToUpper();
        model = await invoiceSerieService.ReplaceOneAsync(id, model);
        return Ok(model);
    }

    [BusinessAuthorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var invoiceSerie = await invoiceSerieService.GetByIdAsync(_companyId, id);
        await invoiceSerieService.DeleteOneAsync(_companyId, invoiceSerie.Id);
        return Ok(invoiceSerie);
    }
}
