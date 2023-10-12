using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Nebula.Common;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Cashier;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.InvoiceHub.Helpers;
using Nebula.Modules.Sales.Comprobantes;
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
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly IValidateStockService _validateStockService;
    private readonly IComprobanteService _comprobanteService;
    private readonly IInvoiceHubService _invoiceHubService;
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly ICashierDetailService _cashierDetailService;

    public InvoiceSaleCashierController(
        ICacheAuthService cacheAuthService,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        IValidateStockService validateStockService,
        IComprobanteService comprobanteService,
        IInvoiceHubService invoiceHubService,
        IInvoiceSaleService invoiceSaleService,
        ICashierDetailService cashierDetailService)
    {
        _cacheAuthService = cacheAuthService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _validateStockService = validateStockService;
        _comprobanteService = comprobanteService;
        _invoiceHubService = invoiceHubService;
        _invoiceSaleService = invoiceSaleService;
        _cashierDetailService = cashierDetailService;
    }

    /// <summary>
    /// registrar venta rápida.
    /// </summary>
    /// <param name="companyId">Identificador Empresa</param>
    /// <param name="model">Modelo Comprobante</param>
    /// <returns>BillingResponse</returns>
    [HttpPost("GenerarVenta")]
    public async Task<IActionResult> GenerarVenta(string companyId, [FromBody] ComprobanteDto model)
    {
        try
        {
            model.Company = await _cacheAuthService.GetCompanyByIdAsync(companyId.Trim());
            var comprobante = await _comprobanteService.SaveChangesAsync(model);
            await _validateStockService.ValidarInvoiceSale(companyId, comprobante.InvoiceSale.Id);

            // registrar operacion en caja.
            if (ObjectId.TryParse(model.Cabecera.CajaDiaria, out ObjectId _))
            {
                var cashierDetail = new CashierDetail()
                {
                    CompanyId = companyId,
                    CajaDiariaId = model.Cabecera.CajaDiaria,
                    InvoiceSaleId = comprobante.InvoiceSale.Id,
                    DocType = comprobante.InvoiceSale.DocType,
                    Document = $"{ comprobante.InvoiceSale.Serie}-{comprobante.InvoiceSale.Number}",
                    ContactId = comprobante.InvoiceSale.ContactId,
                    ContactName = comprobante.InvoiceSale.RznSocialUsuario,
                    Remark = comprobante.InvoiceSale.Remark,
                    TypeOperation = TypeOperationCaja.ComprobanteDeVenta,
                    FormaPago = model.DatoPago.FormaPago,
                    Amount = comprobante.InvoiceSale.SumImpVenta
                };
                await _cashierDetailService.CreateAsync(cashierDetail);
            }

            if (comprobante.InvoiceSale.DocType != "NOTA")
            {
                var invoiceRequest = InvoiceMapper.MapToInvoiceRequestHub(model.Company.Ruc, comprobante);
                var billingResponse = await _invoiceHubService.SendInvoiceAsync(companyId, invoiceRequest);
                comprobante.InvoiceSale.BillingResponse = billingResponse;
                await _invoiceSaleService.UpdateAsync(comprobante.InvoiceSale.Id, comprobante.InvoiceSale);
                return Ok(new { invoiceSaleId = comprobante.InvoiceSale.Id, billingResponse });
            }
            return Ok(new { invoiceSaleId = comprobante.InvoiceSale.Id });
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
