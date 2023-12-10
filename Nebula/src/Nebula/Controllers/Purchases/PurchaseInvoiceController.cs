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

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/purchases/{companyId}/[controller]")]
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

    [HttpGet]
    public async Task<IActionResult> Index(string companyId, [FromQuery] DateQuery query)
    {
        var purchases = await _purchaseInvoiceService.GetAsync(companyId, query);
        return Ok(purchases);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Show(string companyId, string id)
    {
        var purchase = new PurchaseDto
        {
            PurchaseInvoice = await _purchaseInvoiceService.GetByIdAsync(companyId, id),
            PurchaseInvoiceDetails = await _purchaseInvoiceDetailService.GetDetailsAsync(companyId, id),
        };
        return Ok(purchase);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string companyId, [FromBody] CabeceraCompraDto cabecera)
    {
        var purchase = await _purchaseInvoiceService.CreateAsync(companyId, cabecera);
        return Ok(purchase);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] CabeceraCompraDto cabecera)
    {
        var purchase = await _purchaseInvoiceService.UpdateAsync(companyId, id, cabecera);
        return Ok(purchase);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var purchase = await _purchaseInvoiceService.GetByIdAsync(companyId, id);
        await _purchaseInvoiceService.RemoveAsync(companyId, purchase.Id);
        await _purchaseInvoiceDetailService.DeleteManyAsync(companyId, purchase.Id);
        return Ok(purchase);
    }

    [AllowAnonymous]
    [HttpGet("ConsultarValidez")]
    public async Task<IActionResult> ConsultarValidez(string companyId, [FromQuery] QueryConsultarValidezComprobante query)
    {
        string pathArchivoZip = await _validezCompraService.CrearArchivosDeValidacion(companyId, query);
        FileStream stream = new FileStream(pathArchivoZip, FileMode.Open);
        return new FileStreamResult(stream, "application/zip");
    }

    [AllowAnonymous]
    [HttpGet("ExcelRegistroComprasF81")]
    public async Task<IActionResult> ExcelRegistroComprasF81(string companyId, [FromQuery] DateQuery query)
    {
        var purchases = await _purchaseInvoiceService.GetFacturasByMonthAndYearAsync(companyId, query.Month, query.Year);
        // generar archivo excel y enviar como respuesta de solicitud.
        string filePath = new ExcelRegistroComprasF81(purchases).CrearArchivo();
        FileStream stream = new FileStream(filePath, FileMode.Open);
        return new FileStreamResult(stream, ContentTypeFormat.Excel);
    }
}
