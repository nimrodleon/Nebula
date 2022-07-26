using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Services.Sales;
using Nebula.Database.ViewModels.Sales;

namespace Nebula.Controllers.Sales
{
    [Authorize(Roles = AuthRoles.User)]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceSaleController : ControllerBase
    {
        private readonly InvoiceSaleService _invoiceSaleService;
        private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
        private readonly TributoSaleService _tributoSaleService;

        public InvoiceSaleController(InvoiceSaleService invoiceSaleService,
            InvoiceSaleDetailService invoiceSaleDetailService, TributoSaleService tributoSaleService)
        {
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

        [HttpDelete("Delete/{id}"), Authorize(Roles = AuthRoles.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            var invoiceSale = await _invoiceSaleService.GetAsync(id);
            await _invoiceSaleService.RemoveAsync(invoiceSale.Id);
            await _invoiceSaleDetailService.RemoveAsync(invoiceSale.Id);
            await _tributoSaleService.RemoveAsync(invoiceSale.Id);
            return Ok(new { Ok = true, Data = invoiceSale, Msg = "El comprobante de venta ha sido borrado!" });
        }
    }
}
