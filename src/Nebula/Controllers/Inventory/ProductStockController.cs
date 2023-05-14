using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Plugins.Inventory.Stock;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/Inventory/[controller]")]
[ApiController]
public class ProductStockController : ControllerBase
{
    private readonly ProductStockService _productStockService;

    public ProductStockController(ProductStockService productStockService)
    {
        _productStockService = productStockService;
    }

    [Obsolete]
    [HttpGet("StockItemCaja/{invoiceSerieId}/{productId}")]
    public async Task<IActionResult> StockItemCaja(string invoiceSerieId, string productId)
    {
        long quantity = await _productStockService.GetStockItemCajaAsync(invoiceSerieId, productId);
        return Ok(quantity);
    }

    [Obsolete]
    [HttpGet("StockItemComprobante/{warehouseId}/{productId}")]
    public async Task<IActionResult> ItemComprobante(string warehouseId, string productId)
    {
        long quantity = await _productStockService.GetStockItemComprobanteAsync(warehouseId, productId);
        return Ok(quantity);
    }
}
