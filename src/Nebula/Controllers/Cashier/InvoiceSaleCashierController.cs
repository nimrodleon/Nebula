using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Nebula.Modules.Account;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.InvoiceHub.Helpers;
using Nebula.Modules.Sales.Comprobantes;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Models;

namespace Nebula.Controllers.Cashier;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/cashier/[controller]")]
[ApiController]
public class InvoiceSaleCashierController(
    IUserAuthenticationService userAuthenticationService,
    ICompanyService companyService,
    IInvoiceSaleDetailService invoiceSaleDetailService,
    // IValidateStockService validateStockService,
    IComprobanteService comprobanteService,
    IInvoiceHubService invoiceHubService,
    IInvoiceSaleService invoiceSaleService,
    ICashierDetailService cashierDetailService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    /// <summary>
    /// registrar venta rápida.
    /// </summary>
    /// <param name="companyId">Identificador Empresa</param>
    /// <param name="model">Modelo Comprobante</param>
    /// <returns>BillingResponse</returns>
    [HttpPost("GenerarVenta")]
    public async Task<IActionResult> GenerarVenta([FromBody] ComprobanteDto model)
    {
        try
        {
            var company = await companyService.GetByIdAsync(_companyId.Trim());
            var comprobante = await comprobanteService.SaveChangesAsync(company, model);
            // await validateStockService.ValidarInvoiceSale(comprobante);

            // registrar operacion en caja.
            if (ObjectId.TryParse(model.Cabecera.CajaDiariaId, out ObjectId _))
            {
                var cashierDetail = new CashierDetail()
                {
                    CompanyId = _companyId.Trim(),
                    CajaDiariaId = model.Cabecera.CajaDiariaId,
                    InvoiceSaleId = comprobante.InvoiceSale.Id,
                    DocType = comprobante.InvoiceSale.TipoDoc,
                    Document = $"{comprobante.InvoiceSale.Serie}-{comprobante.InvoiceSale.Correlativo}",
                    ContactId = comprobante.InvoiceSale.ContactId,
                    ContactName = comprobante.InvoiceSale.Cliente.RznSocial,
                    Remark = comprobante.InvoiceSale.Remark,
                    TypeOperation = TipoOperationCaja.ComprobanteDeVenta,
                    FormaPago = model.Cabecera.PaymentMethod,
                    Amount = comprobante.InvoiceSale.MtoImpVenta
                };
                await cashierDetailService.InsertOneAsync(cashierDetail);
            }

            if (comprobante.InvoiceSale.TipoDoc != "NOTA")
            {
                var invoiceRequest = InvoiceMapper.MapToInvoiceRequestHub(company.Ruc, comprobante);
                var billingResponse = await invoiceHubService.SendInvoiceAsync(_companyId, invoiceRequest);
                comprobante.InvoiceSale.BillingResponse = billingResponse;
                await invoiceSaleService.ReplaceOneAsync(comprobante.InvoiceSale.Id, comprobante.InvoiceSale);
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
    public async Task<IActionResult> ProductReport(string id)
    {
        List<InvoiceSaleDetail> invoiceSaleDetails =
            await invoiceSaleDetailService.GetItemsByCajaDiaria(_companyId, id);
        return Ok(invoiceSaleDetails);
    }
}
