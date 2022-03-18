using Microsoft.AspNetCore.Mvc;
using Nebula.Data;
using Nebula.Data.Models;
using Nebula.Data.Services;
using Nebula.Data.ViewModels;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace Nebula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceSaleController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IRavenDbContext _context;
    private readonly ISaleService _saleService;
    private readonly ICpeService _cpeService;

    public InvoiceSaleController(ILogger<InvoiceSaleController> logger,
        IRavenDbContext context, ISaleService saleService, ICpeService cpeService)
    {
        _logger = logger;
        _context = context;
        _saleService = saleService;
        _cpeService = cpeService;
    }

    [HttpGet("Index/{type}")]
    public async Task<IActionResult> Index([FromQuery] VoucherQuery model)
    {
        using var session = _context.Store.OpenAsyncSession();
        IRavenQueryable<InvoiceSale> invoiceSales = from m in session.Query<InvoiceSale>()
                .Where(x => x.Year == model.Year && x.Month == model.Month)
                                                    select m;
        if (!string.IsNullOrWhiteSpace(model.Query))
            invoiceSales = invoiceSales.Search(m => m.RznSocialUsuario, $"*{model.Query.ToUpper()}*");
        var responseData = await invoiceSales.ToListAsync();
        return Ok(responseData);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        using var session = _context.Store.OpenAsyncSession();
        var responseData = new ResponseInvoiceSale()
        {
            InvoiceSale = await session.LoadAsync<InvoiceSale>(id),
            InvoiceSaleDetails = await session.Query<InvoiceSaleDetail>()
                .Where(m => m.InvoiceSale == id).ToListAsync(),
            TributoSales = await session.Query<TributoSale>()
                .Where(m => m.InvoiceSale == id).ToListAsync(),
            InvoiceSaleAccounts = await session.Query<InvoiceSaleAccount>()
                .Where(m => m.InvoiceSale == id).ToListAsync()
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
            _saleService.SetModel(model);
            var invoiceSale = await _saleService.CreateQuickSale(id);
            bool fileExist = false;

            // generar fichero JSON según tipo comprobante.
            switch (invoiceSale.DocType)
            {
                case "BOLETA":
                    fileExist = await _cpeService.CreateBoletaJson(invoiceSale.Id);
                    break;
                case "FACTURA":
                    fileExist = await _cpeService.CreateFacturaJson(invoiceSale.Id);
                    break;
                case "NOTA":
                    fileExist = true;
                    break;
            }

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
            _logger.LogError(e.Message);
            return BadRequest(new { Ok = false, Msg = e.Message });
        }
    }

    // /// <summary>
    // /// registrar comprobante de venta.
    // /// </summary>
    // /// <param name="id">ID serie de facturación</param>
    // /// <param name="model">Comprobante</param>
    // /// <returns>IActionResult</returns>
    // [HttpPost("CreateSale/{id}")]
    // public async Task<IActionResult> CreateSale(int id, [FromBody] Comprobante model)
    // {
    //     try
    //     {
    //         _comprobanteService.SetModel(model);
    //         var invoice = await _comprobanteService.CreateSale(id);
    //         model.InvoiceId = invoice.Id;
    //         bool fileExist = false;
    //
    //         // generar fichero JSON según tipo comprobante.
    //         switch (invoice.DocType)
    //         {
    //             case "BOLETA":
    //                 fileExist = await _cpeService.CreateBoletaJson(invoice.Id);
    //                 break;
    //             case "FACTURA":
    //                 fileExist = await _cpeService.CreateFacturaJson(invoice.Id);
    //                 break;
    //         }
    //
    //         return Ok(new
    //         {
    //             Ok = fileExist, Data = model,
    //             Msg = $"{invoice.Serie} - {invoice.Number} ha sido registrado!"
    //         });
    //     }
    //     catch (Exception e)
    //     {
    //         _logger.LogError(e.Message);
    //         return BadRequest(new {Ok = false, Msg = e.Message});
    //     }
    // }
}
