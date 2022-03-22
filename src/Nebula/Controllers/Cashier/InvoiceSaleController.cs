using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Services.Cashier;
using Nebula.Data.Services.Sales;
using Nebula.Data.ViewModels.Cashier;

namespace Nebula.Controllers.Cashier;

[Route("api/[controller]")]
[ApiController]
public class InvoiceSaleController : ControllerBase
{
    private readonly CashierSaleService _cashierSaleService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;
    private readonly InvoiceSaleAccountService _invoiceSaleAccountService;

    public InvoiceSaleController(CashierSaleService cashierSaleService, InvoiceSaleService invoiceSaleService,
        InvoiceSaleDetailService invoiceSaleDetailService, TributoSaleService tributoSaleService,
        InvoiceSaleAccountService invoiceSaleAccountService)
    {
        _cashierSaleService = cashierSaleService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
        _invoiceSaleAccountService = invoiceSaleAccountService;
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var responseData = new ResponseInvoiceSale()
        {
            InvoiceSale = await _invoiceSaleService.GetAsync(id),
            InvoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(id),
            TributoSales = await _tributoSaleService.GetListAsync(id),
            InvoiceSaleAccounts = await _invoiceSaleAccountService.GetListAsync(id)
        };
        return Ok(responseData);
    }

    /// <summary>
    /// registrar venta rápida.
    /// </summary>
    /// <param name="id">ID caja diaria</param>
    /// <param name="model">Venta</param>
    /// <returns>IActionResult</returns>
    [HttpPost("CreateQuickSale/{id}")]
    public async Task<IActionResult> CreateQuickSale(string id, [FromBody] Venta model)
    {
        try
        {
            _cashierSaleService.SetModel(model);
            var invoiceSale = await _cashierSaleService.CreateQuickSale(id);
            bool fileExist = true;

            // Configurar valor de retorno.
            model.InvoiceSale = invoiceSale.Id;
            model.Vuelto = model.MontoTotal - model.SumImpVenta;

            return Ok(new
            {
                Ok = fileExist,
                Data = model,
                Msg = $"El comprobante {invoiceSale.Serie}-{invoiceSale.Number} ha sido registrado!"
            });
        }
        catch (Exception e)
        {
            return BadRequest(new {Ok = false, Msg = e.Message});
        }
    }
}
