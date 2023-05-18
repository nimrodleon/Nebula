using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Sales;
using Nebula.Database.Services.Cashier;
using Nebula.Database.Services.Facturador;
using Nebula.Database.Services.Sales;
using Nebula.Database.Dto.Sales;
using Nebula.Database.Services.Common;
using Nebula.Modules.Inventory.Stock;

namespace Nebula.Controllers.Cashier;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class InvoiceSaleCashierController : ControllerBase
{
    private readonly CashierSaleService _cashierSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly ValidateStockService _validateStockService;
    private readonly FacturadorService _facturadorService;
    private readonly ConfigurationService _configurationService;

    public InvoiceSaleCashierController(CashierSaleService cashierSaleService,
        InvoiceSaleDetailService invoiceSaleDetailService, ValidateStockService validateStockService,
        FacturadorService facturadorService, ConfigurationService configurationService)
    {
        _cashierSaleService = cashierSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _validateStockService = validateStockService;
        _facturadorService = facturadorService;
        _configurationService = configurationService;
    }

    /// <summary>
    /// registrar venta rápida.
    /// </summary>
    /// <param name="id">ID caja diaria</param>
    /// <param name="model">GenerarVenta</param>
    /// <returns>IActionResult</returns>
    [HttpPost("GenerarVenta/{id}")]
    public async Task<IActionResult> GenerarVenta(string id, [FromBody] ComprobanteDto model)
    {
        try
        {
            var license = await _configurationService.ValidarAcceso();
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
    [HttpGet("ProductReport/{id}")]
    public async Task<IActionResult> ProductReport(string id)
    {
        List<InvoiceSaleDetail> invoiceSaleDetails = await _invoiceSaleDetailService.GetItemsByCajaDiaria(id);
        return Ok(invoiceSaleDetails);
    }
}
