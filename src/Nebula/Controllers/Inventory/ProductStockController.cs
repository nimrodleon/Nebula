using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Database.Helpers;
using Nebula.Plugins.Inventory.Stock;
using Nebula.Plugins.Inventory.Stock.Dto;

namespace Nebula.Controllers.Inventory;

[Authorize(Roles = AuthRoles.User)]
[Route("api/Inventory/[controller]")]
[ApiController]
public class ProductStockController : ControllerBase
{
    private readonly ProductStockService _productStockService;
    private readonly IHelperCalculateProductStockService _helperCalculateProductStockService;

    public ProductStockController(ProductStockService productStockService,
        IHelperCalculateProductStockService helperCalculateProductStockService)
    {
        _productStockService = productStockService;
        _helperCalculateProductStockService = helperCalculateProductStockService;
    }

    [HttpGet("GetStockInfos/{productId}")]
    public async Task<IActionResult> GetStockInfos(string productId)
    {
        var responseData = await _helperCalculateProductStockService.GetProductStockInfos(productId);
        return Ok(responseData);
    }

    [HttpPost("ChangeQuantity"), Authorize(Roles = AuthRoles.Admin)]
    public async Task<IActionResult> ChangeQuantity([FromBody] ChangeQuantityStockRequestParams requestParams)
    {
        var productStock = await _productStockService.ChangeQuantity(requestParams);
        return Ok(productStock);
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
