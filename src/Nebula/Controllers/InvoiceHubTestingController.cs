using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.InvoiceHub.Dto;

namespace Nebula.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceHubTestingController : ControllerBase
    {
        private readonly IInvoiceHubService _invoiceHubService;

        public InvoiceHubTestingController(IInvoiceHubService invoiceHubService)
        {
            _invoiceHubService = invoiceHubService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var invoiceRequest = new InvoiceRequestHub
            {
                Ruc = "20520485750",
                TipoOperacion = "0101",
                TipoDoc = "01",
                Serie = "F001",
                Correlativo = "1",
                FormaPago = new FormaPagoHub
                {
                    Moneda = "PEN",
                    Tipo = "Contado",
                    Monto = 100.0000M,
                },
                TipoMoneda = "PEN",
                Client = new ClientHub
                {
                    TipoDoc = "6",
                    NumDoc = "10234545455",
                    RznSocial = "Cliente Ejemplo SRL",
                },
                Details = new List<DetailHub>
            {
                new DetailHub
                {
                    CodProducto = "6519b9191d1a04bb07c280b2",
                    Unidad = "NIU",
                    Cantidad = 2,
                    MtoValorUnitario = 50.0000M,
                    Descripcion = "Producto 1",
                    MtoBaseIgv = 100.0000M,
                    PorcentajeIgv = 18,
                    Igv = 18,
                    TipAfeIgv = "10",
                    TotalImpuestos = 18,
                    MtoValorVenta = 100.0000M,
                    MtoPrecioUnitario = 59.0000M,
                },
            },
            };
            var result = await _invoiceHubService.SendInvoiceAsync("6519b9191d1a04bb07c280b2", invoiceRequest);
            return Ok(result);
        }
    }
}
