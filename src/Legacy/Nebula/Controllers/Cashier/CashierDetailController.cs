using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Cashier.Models;
using Nebula.Common.Helpers;

namespace Nebula.Controllers.Cashier;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/cashier/{companyId}/[controller]")]
[ApiController]
public class CashierDetailController : ControllerBase
{
    private readonly ICashierDetailService _cashierDetailService;

    public CashierDetailController(ICashierDetailService cashierDetailService)
    {
        _cashierDetailService = cashierDetailService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id, [FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var detallesCaja = await _cashierDetailService.GetDetalleCajaDiariaAsync(companyId, id, query, page, pageSize);
        var totalDetallesCaja = await _cashierDetailService.GetTotalDetalleCajaDiariaAsync(companyId, id, query);
        var totalPages = (int)Math.Ceiling((double)totalDetallesCaja / pageSize);

        var paginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages
        };

        paginationInfo.GeneratePageLinks();

        var result = new PaginationResult<CashierDetail>
        {
            Pagination = paginationInfo,
            Data = detallesCaja
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] CashierDetail model)
    {
        if (model.TypeOperation == TipoOperationCaja.EntradaDeDinero)
            model.TypeOperation = TipoOperationCaja.EntradaDeDinero;
        if (model.TypeOperation == TipoOperationCaja.SalidaDeDinero)
            model.TypeOperation = TipoOperationCaja.SalidaDeDinero;
        model.FormaPago = MetodosPago.Contado;
        model.CompanyId = companyId.Trim();
        model = await _cashierDetailService.CreateAsync(model);
        return Ok(model);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await _cashierDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }

    [HttpGet("ResumenCaja/{id}")]
    public async Task<IActionResult> ResumenCaja(string companyId, string id)
    {
        var resumenCaja = await _cashierDetailService.GetResumenCaja(companyId, id);
        return Ok(resumenCaja);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = CompanyRoles.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var cashierDetail = await _cashierDetailService.GetByIdAsync(companyId, id);
        await _cashierDetailService.RemoveAsync(companyId, cashierDetail.Id);
        return Ok(cashierDetail);
    }
}
