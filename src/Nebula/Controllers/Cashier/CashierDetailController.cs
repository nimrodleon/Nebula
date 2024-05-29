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
[Route("api/cashier/[controller]")]
[ApiController]
public class CashierDetailController(
    IUserAuthenticationService userAuthenticationService,
    ICashierDetailService cashierDetailService) : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id, [FromQuery] string query = "", [FromQuery] int page = 1)
    {
        int pageSize = 12;
        var detallesCaja = await cashierDetailService.GetDetalleCajaDiariaAsync(_companyId, id, query, page, pageSize);
        var totalDetallesCaja = await cashierDetailService.GetTotalDetalleCajaDiariaAsync(_companyId, id, query);
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
    public async Task<IActionResult> Create([FromBody] CashierDetail model)
    {
        if (model.TypeOperation == TipoOperationCaja.EntradaDeDinero)
            model.TypeOperation = TipoOperationCaja.EntradaDeDinero;
        if (model.TypeOperation == TipoOperationCaja.SalidaDeDinero)
            model.TypeOperation = TipoOperationCaja.SalidaDeDinero;
        model.FormaPago = MetodosPago.Contado;
        model.CompanyId = _companyId.Trim();
        model = await cashierDetailService.InsertOneAsync(model);
        return Ok(model);
    }

    [HttpGet("CountDocuments/{id}")]
    public async Task<IActionResult> CountDocuments(string id)
    {
        var countDocuments = await cashierDetailService.CountDocumentsAsync(_companyId, id);
        return Ok(countDocuments);
    }

    [HttpGet("ResumenCaja/{id}")]
    public async Task<IActionResult> ResumenCaja(string id)
    {
        var resumenCaja = await cashierDetailService.GetResumenCaja(_companyId, id);
        return Ok(resumenCaja);
    }

    [HttpDelete("{id}"), CustomerAuthorize(UserRole = UserRoleHelper.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var cashierDetail = await cashierDetailService.GetByIdAsync(_companyId, id);
        await cashierDetailService.DeleteOneAsync(_companyId, cashierDetail.Id);
        return Ok(cashierDetail);
    }
}
