using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Cashier;
using Nebula.Modules.Configurations.Subscriptions;
using Nebula.Modules.Facturador;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Models;

namespace Nebula.Controllers.Cashier;

[Route("api/[controller]")]
[ApiController]
public class InvoiceSaleCashierController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ICashierSaleService _cashierSaleService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly IValidateStockService _validateStockService;
    private readonly IFacturadorService _facturadorService;

    public InvoiceSaleCashierController(ISubscriptionService subscriptionService,
        ICashierSaleService cashierSaleService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        IValidateStockService validateStockService,
        IFacturadorService facturadorService)
    {
        _subscriptionService = subscriptionService;
        _cashierSaleService = cashierSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _validateStockService = validateStockService;
        _facturadorService = facturadorService;
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
            var license = await _subscriptionService.ValidarAcceso();
            if (!license.Ok) throw new Exception("Error, Verificar suscripción!");
            _cashierSaleService.SetComprobanteDto(model);
            var invoiceSale = await _cashierSaleService.SaveChangesAsync(id);
            if (invoiceSale.DocType != "NOTA")
                await _facturadorService.JsonInvoiceParser(invoiceSale.Id);

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
