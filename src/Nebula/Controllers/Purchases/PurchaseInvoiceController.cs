using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Purchases;
using Nebula.Modules.Purchases.Dto;

namespace Nebula.Controllers.Purchases;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class PurchaseInvoiceController : ControllerBase
{
    private readonly IPurchaseInvoiceService _purchaseInvoiceService;
    private readonly IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;

    public PurchaseInvoiceController(IPurchaseInvoiceService purchaseInvoiceService,
        IPurchaseInvoiceDetailService purchaseInvoiceDetailService)
    {
        _purchaseInvoiceService = purchaseInvoiceService;
        _purchaseInvoiceDetailService = purchaseInvoiceDetailService;
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
}
