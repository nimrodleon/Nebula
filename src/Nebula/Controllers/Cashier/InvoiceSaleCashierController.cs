using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Cashier;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Models;

namespace Nebula.Controllers.Cashier;

[Route("api/[controller]")]
[ApiController]
public class InvoiceSaleCashierController : ControllerBase
{
    private readonly ICashierSaleService _cashierSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly IValidateStockService _validateStockService;

    public InvoiceSaleCashierController(
        ICashierSaleService cashierSaleService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        IValidateStockService validateStockService)
    {
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
    [HttpPost("GenerarVenta/{id}"), UserAuthorize(Permission.PosCreate)]
    public async Task<IActionResult> GenerarVenta(string id, [FromBody] ComprobanteDto model)
    {
        try
        {
            model.UserCompany = new Modules.Account.Models.Company();
            var invoiceSale = await _cashierSaleService.SaveChangesAsync(model, id);
            // if (invoiceSale.DocType != "NOTA")
            // pass...

            // Validar Inventario.
            await _validateStockService.ValidarInvoiceSale(invoiceSale.Id);

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
    [HttpGet("ProductReport/{id}"), UserAuthorize(Permission.PosRead)]
    public async Task<IActionResult> ProductReport(string id)
    {
        List<InvoiceSaleDetail> invoiceSaleDetails = await _invoiceSaleDetailService.GetItemsByCajaDiaria(id);
        return Ok(invoiceSaleDetails);
    }
}
