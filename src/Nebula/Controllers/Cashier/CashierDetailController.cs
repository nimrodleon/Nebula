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
[CustomerAuthorize(UserRole = UserRoleHelper.User)]
[Route("api/cashier/{companyId}/[controller]")]
[ApiController]
public class CashierDetailController(ICashierDetailService cashierDetailService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string companyId, string id, [FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var detallesCaja = await cashierDetailService.GetDetalleCajaDiariaAsync(companyId, id, query, page, pageSize);
        var totalDetallesCaja = await cashierDetailService.GetTotalDetalleCajaDiariaAsync(companyId, id, query);
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
        model = await cashierDetailService.InsertOneAsync(model);
        return Ok(model);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string companyId, string id)
    {
        var countDocuments = await cashierDetailService.CountDocumentsAsync(companyId, id);
        return Ok(countDocuments);
    }

    [HttpGet("ResumenCaja/{id}")]
    public async Task<IActionResult> ResumenCaja(string companyId, string id)
    {
        var resumenCaja = await cashierDetailService.GetResumenCaja(companyId, id);
        return Ok(resumenCaja);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var cashierDetail = await cashierDetailService.GetByIdAsync(companyId, id);
        await cashierDetailService.DeleteOneAsync(companyId, cashierDetail.Id);
        return Ok(cashierDetail);
    }
}
