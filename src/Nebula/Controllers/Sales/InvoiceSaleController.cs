using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Data.Helpers;
using Nebula.Data.Services.Sales;
using Nebula.Data.ViewModels.Sales;

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
    }
}
