using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Cashier;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Models;

namespace Nebula.Controllers.Cashier;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/cashier/{companyId}/[controller]")]
[ApiController]
public class InvoiceSaleCashierController : ControllerBase
{
    private readonly ICacheAuthService _cacheAuthService;
    private readonly ICashierSaleService _cashierSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly IValidateStockService _validateStockService;

    public InvoiceSaleCashierController(
        ICacheAuthService cacheAuthService,
        ICashierSaleService cashierSaleService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        IValidateStockService validateStockService)
    {
        _cacheAuthService = cacheAuthService;
        _cashierSaleService = cashierSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _validateStockService = validateStockService;
    }

    /// <summary>
    /// registrar venta rápida.
    /// </summary>
    /// <param name="id">ID caja diaria</param>
    /// <param name="model">GenerarVenta</param>
    /// <returns>IActionResult</returns>
    [HttpPost("GenerarVenta/{id}")]
    public async Task<IActionResult> GenerarVenta(string companyId, string id, [FromBody] ComprobanteDto model)
    {
        try
        {
            model.Company = await _cacheAuthService.GetCompanyByIdAsync(companyId.Trim());
            var invoiceSale = await _cashierSaleService.SaveChangesAsync(model, id);
            // if (invoiceSale.DocType != "NOTA")
            // pass...

            // Validar Inventario.
            await _validateStockService.ValidarInvoiceSale(companyId, invoiceSale.Id);

            return Ok(invoiceSale);
        }
        catch (Exception e)
        {
            return BadRequest(new { Ok = false, Msg = e.Message });
        }
    }

    /// <summary>
    /// Lista de productos vendidos.
    /// </summary>
    /// <param name="id">ID CajaDiaria</param>
    /// <returns>Lista de Productos</returns>
    [HttpGet("ProductReport/{id}")]
    public async Task<IActionResult> ProductReport(string companyId, string id)
    {
        List<InvoiceSaleDetail> invoiceSaleDetails = await _invoiceSaleDetailService.GetItemsByCajaDiaria(companyId, id);
        return Ok(invoiceSaleDetails);
    }
}
