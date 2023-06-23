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

    public PurchaseInvoiceController(IPurchaseInvoiceService purchaseInvoiceService)
    {
        _purchaseInvoiceService = purchaseInvoiceService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CabeceraCompraDto cabecera)
    {
        var purchase = await _purchaseInvoiceService.CreateAsync(cabecera);
        return Ok(purchase);
    }
}
