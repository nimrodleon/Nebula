using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Helpers;
using Nebula.Data.Services.Cashier;
using Nebula.Data.Services.Sales;
using Nebula.Data.ViewModels.Cashier;

namespace Nebula.Controllers.Cashier;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class InvoiceSaleCashierController : ControllerBase
{
    private readonly CashierSaleService _cashierSaleService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;

    public InvoiceSaleCashierController(CashierSaleService cashierSaleService, InvoiceSaleService invoiceSaleService,
        InvoiceSaleDetailService invoiceSaleDetailService, TributoSaleService tributoSaleService)
    {
        _cashierSaleService = cashierSaleService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var responseData = new ResponseInvoiceSale()
        {
            InvoiceSale = await _invoiceSaleService.GetAsync(id),
            InvoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(id),
            TributoSales = await _tributoSaleService.GetListAsync(id)
        };
        return Ok(responseData);
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
            return BadRequest(new {Ok = false, Msg = e.Message});
        }
    }
}
