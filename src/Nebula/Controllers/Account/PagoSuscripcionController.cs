using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Account;
using Microsoft.AspNetCore.Authorization;
using Nebula.Common.Helpers;
using Nebula.Modules.Account.Models;

namespace Nebula.Controllers.Account;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/account/[controller]")]
[ApiController]
public class PagoSuscripcionController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly IPagoSuscripcionService _pagoSuscripcionService;

    public PagoSuscripcionController(
        ICompanyService companyService,
        IPagoSuscripcionService pagoSuscripcionService)
    {
        _companyService = companyService;
        _pagoSuscripcionService = pagoSuscripcionService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Index(
        string userId,
        [FromQuery] string year = "2024",
        [FromQuery] string query = "",
        [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var pagos = await _pagoSuscripcionService.GetSuscripcionesAsync(
            userId, year, query, page, pageSize);
        var totalPagos = await _pagoSuscripcionService.GetTotalSuscripcionesAsync(userId, year, query);
        var totalPages = (int)Math.Ceiling((double)totalPagos / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<PagoSuscripcion>
        {
            Pagination = paginationInfo,
            Data = pagos
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PagoSuscripcion model)
    {
        await _pagoSuscripcionService.CreateAsync(model);
        var company = await _companyService.GetByIdAsync(model.CompanyId);
        if (company != null)
        {
            company.PagoSuscripcionId = model.Id;
            await _companyService.UpdateAsync(company.Id, company);
        }
        return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] PagoSuscripcion model)
    {
        model = await _pagoSuscripcionService.UpdateAsync(id, model);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var pagoSuscripcion = await _pagoSuscripcionService.GetByIdAsync(id);
        await _pagoSuscripcionService.RemoveAsync(pagoSuscripcion.Id);
        return Ok(pagoSuscripcion);
    }
}
