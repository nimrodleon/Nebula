using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Database.Services.Inventory;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/[controller]")]
[ApiController]
public class ProductStockController : ControllerBase
{
    private readonly ProductStockService _productStockService;

    public ProductStockController(ProductStockService productStockService)
    {
        _productStockService = productStockService;
    }

    [HttpGet("ItemComprobante/{invoiceSerieId}/{productId}")]
    public async Task<IActionResult> ItemComprobante(string invoiceSerieId, string productId)
    {
        long quantity = await _productStockService.GetStockItemComprobanteAsync(invoiceSerieId, productId);
        return Ok(quantity);
    }
}
