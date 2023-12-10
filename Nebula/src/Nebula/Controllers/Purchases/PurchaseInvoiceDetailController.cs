using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Purchases;
using Nebula.Modules.Purchases.Dto;

namespace Nebula.Controllers.Purchases;

[Authorize]
[CustomerAuthorize(UserRole = CompanyRoles.User)]
[Route("api/purchases/{companyId}/[controller]")]
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

    [HttpPost("{purchaseInvoiceId}")]
    public async Task<IActionResult> Create(string companyId, string purchaseInvoiceId, [FromBody] ItemCompraDto itemCompra)
    {
        var purchaseInvoiceDetail = await _purchaseInvoiceDetailService.CreateAsync(companyId, purchaseInvoiceId, itemCompra);
        var details = await _purchaseInvoiceDetailService.GetDetailsAsync(companyId, purchaseInvoiceId);
        await _purchaseInvoiceService.UpdateImporteAsync(companyId, purchaseInvoiceId, details);
        return Ok(purchaseInvoiceDetail);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string companyId, string id, [FromBody] ItemCompraDto itemCompra)
    {
        var purchaseInvoiceDetail = await _purchaseInvoiceDetailService.UpdateAsync(companyId, id, itemCompra);
        var details = await _purchaseInvoiceDetailService.GetDetailsAsync(companyId, purchaseInvoiceDetail.PurchaseInvoiceId);
        await _purchaseInvoiceService.UpdateImporteAsync(companyId, purchaseInvoiceDetail.PurchaseInvoiceId, details);
        return Ok(purchaseInvoiceDetail);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string companyId, string id)
    {
        var itemCompra = await _purchaseInvoiceDetailService.GetByIdAsync(companyId, id);
        await _purchaseInvoiceDetailService.RemoveAsync(companyId, itemCompra.Id);
        var details = await _purchaseInvoiceDetailService.GetDetailsAsync(companyId, itemCompra.PurchaseInvoiceId);
        await _purchaseInvoiceService.UpdateImporteAsync(companyId, itemCompra.PurchaseInvoiceId, details);
        return Ok(itemCompra);
    }
}
