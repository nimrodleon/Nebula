using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Common.Dto;
using Nebula.Common.Helpers;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Purchases;
using Nebula.Modules.Purchases.Dto;
using Nebula.Modules.Purchases.Helpers;
using Nebula.Modules.Sales;

namespace Nebula.Controllers.Purchases;

[Route("api/[controller]")]
[ApiController]
public class PurchaseInvoiceController : ControllerBase
{
    private readonly IPurchaseInvoiceService _purchaseInvoiceService;
    private readonly IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
    private readonly IConsultarValidezCompraService _validezCompraService;

    public PurchaseInvoiceController(IPurchaseInvoiceService purchaseInvoiceService,
        IPurchaseInvoiceDetailService purchaseInvoiceDetailService,
        IConsultarValidezCompraService validezCompraService)
    {
        _purchaseInvoiceService = purchaseInvoiceService;
        _purchaseInvoiceDetailService = purchaseInvoiceDetailService;
        _validezCompraService = validezCompraService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] DateQuery query)
    {
        var purchases = await _purchaseInvoiceService.GetAsync(query);
        return Ok(purchases);
    }

    [HttpGet("Show/{id}")]
    public async Task<IActionResult> Show(string id)
    {
        var purchase = new PurchaseDto
        {
            PurchaseInvoice = await _purchaseInvoiceService.GetByIdAsync(id),
            PurchaseInvoiceDetails = await _purchaseInvoiceDetailService.GetDetailsAsync(id),
        };
        return Ok(purchase);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CabeceraCompraDto cabecera)
    {
        var purchase = await _purchaseInvoiceService.CreateAsync(cabecera);
        return Ok(purchase);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CabeceraCompraDto cabecera)
    {
        var purchase = await _purchaseInvoiceService.UpdateAsync(id, cabecera);
        return Ok(purchase);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var purchase = await _purchaseInvoiceService.GetByIdAsync(id);
        await _purchaseInvoiceService.RemoveAsync(purchase.Id);
        await _purchaseInvoiceDetailService.DeleteManyAsync(purchase.Id);
        return Ok(purchase);
    }

    [AllowAnonymous]
    [HttpGet("ConsultarValidez")]
    public async Task<IActionResult> ConsultarValidez([FromQuery] QueryConsultarValidezComprobante query)
    {
        string pathArchivoZip = await _validezCompraService.CrearArchivosDeValidacion(query);
        FileStream stream = new FileStream(pathArchivoZip, FileMode.Open);
        return new FileStreamResult(stream, "application/zip");
    }

    [AllowAnonymous]
    [HttpGet("ExcelRegistroComprasF81")]
    public async Task<IActionResult> ExcelRegistroComprasF81([FromQuery] DateQuery query)
    {
        var purchases = await _purchaseInvoiceService.GetFacturasByMonthAndYearAsync(query.Month, query.Year);
        // generar archivo excel y enviar como respuesta de solicitud.
        string filePath = new ExcelRegistroComprasF81(purchases).CrearArchivo();
        FileStream stream = new FileStream(filePath, FileMode.Open);
        return new FileStreamResult(stream, ContentTypeFormat.Excel);
    }
}
