using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Purchases;
using Nebula.Modules.Purchases.Dto;

namespace Nebula.Controllers.Purchases;

[Route("api/[controller]")]
[ApiController]
public class PurchaseInvoiceDetailController : ControllerBase
{
    private readonly IPurchaseInvoiceService _purchaseInvoiceService;
    private readonly IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;

    public PurchaseInvoiceDetailController(IPurchaseInvoiceService purchaseInvoiceService,
        IPurchaseInvoiceDetailService purchaseInvoiceDetailService)
    {
        _purchaseInvoiceService = purchaseInvoiceService;
        _purchaseInvoiceDetailService = purchaseInvoiceDetailService;
    }

    [HttpPost("Create/{purchaseInvoiceId}"), UserAuthorize(Permission.PurchasesCreate)]
    public async Task<IActionResult> Create(string purchaseInvoiceId, [FromBody] ItemCompraDto itemCompra)
    {
        var purchaseInvoiceDetail = await _purchaseInvoiceDetailService.CreateAsync(purchaseInvoiceId, itemCompra);
        var details = await _purchaseInvoiceDetailService.GetDetailsAsync(purchaseInvoiceId);
        await _purchaseInvoiceService.UpdateImporteAsync(purchaseInvoiceId, details);
        return Ok(purchaseInvoiceDetail);
    }

    [HttpPut("Update/{id}"), UserAuthorize(Permission.PurchasesEdit)]
    public async Task<IActionResult> Update(string id, [FromBody] ItemCompraDto itemCompra)
    {
        var purchaseInvoiceDetail = await _purchaseInvoiceDetailService.UpdateAsync(id, itemCompra);
        var details = await _purchaseInvoiceDetailService.GetDetailsAsync(purchaseInvoiceDetail.PurchaseInvoiceId);
        await _purchaseInvoiceService.UpdateImporteAsync(purchaseInvoiceDetail.PurchaseInvoiceId, details);
        return Ok(purchaseInvoiceDetail);
    }

    [HttpDelete("Delete/{id}"), UserAuthorize(Permission.PurchasesDelete)]
    public async Task<IActionResult> Delete(string id)
    {
        var itemCompra = await _purchaseInvoiceDetailService.GetByIdAsync(id);
        await _purchaseInvoiceDetailService.RemoveAsync(itemCompra.Id);
        var details = await _purchaseInvoiceDetailService.GetDetailsAsync(itemCompra.PurchaseInvoiceId);
        await _purchaseInvoiceService.UpdateImporteAsync(itemCompra.PurchaseInvoiceId, details);
        return Ok(itemCompra);
    }
}
