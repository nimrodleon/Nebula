using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Sales;
using Nebula.Database.Services.Cashier;
using Nebula.Database.Services.Sales;
using Nebula.Database.ViewModels.Cashier;

namespace Nebula.Controllers.Cashier;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class InvoiceSaleCashierController : ControllerBase
{
    private readonly CashierSaleService _cashierSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;

    public InvoiceSaleCashierController(CashierSaleService cashierSaleService,
        InvoiceSaleDetailService invoiceSaleDetailService)
    {
        _cashierSaleService = cashierSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
    }

    /// <summary>
    /// registrar venta rápida.
    /// </summary>
    /// <param name="id">ID caja diaria</param>
    /// <param name="model">GenerarVenta</param>
    /// <returns>IActionResult</returns>
    [HttpPost("GenerarVenta/{id}")]
    public async Task<IActionResult> GenerarVenta(string id, [FromBody] GenerarVenta model)
    {
        try
        {
            _cashierSaleService.SetModel(model);
            var invoiceSale = await _cashierSaleService.SaveChanges(id);
            bool fileExist = true;

            // Configurar valor de retorno.
            model.Comprobante.InvoiceSale = invoiceSale.Id;
            model.Comprobante.Vuelto = model.Comprobante.MontoRecibido - model.Comprobante.SumImpVenta;

            return Ok(new
            {
                Ok = fileExist,
                Data = model.Comprobante,
                Msg = $"El comprobante {invoiceSale.Serie}-{invoiceSale.Number} ha sido registrado!"
            });
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
