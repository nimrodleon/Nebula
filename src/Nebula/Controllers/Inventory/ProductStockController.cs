using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Helpers;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Stock.Dto;

namespace Nebula.Controllers.Inventory;

[Route("api/inventory/{companyId}/[controller]")]
[ApiController]
public class ProductStockController : ControllerBase
{
    private readonly IProductStockService _productStockService;
    private readonly IHelperCalculateProductStockService _helperCalculateProductStockService;

    public ProductStockController(IProductStockService productStockService,
        IHelperCalculateProductStockService helperCalculateProductStockService)
    {
        _productStockService = productStockService;
        _helperCalculateProductStockService = helperCalculateProductStockService;
    }

    [HttpGet("GetStockInfos/{productId}")]
    public async Task<IActionResult> GetStockInfos(string companyId, string productId)
    {
        var responseData = await _helperCalculateProductStockService.GetProductStockInfos(companyId, productId);
        return Ok(responseData);
    }

    [HttpPost("ChangeQuantity")]
    public async Task<IActionResult> ChangeQuantity(string companyId, [FromBody] ChangeQuantityStockRequestParams requestParams)
    {
        var productStock = await _productStockService.ChangeQuantity(companyId, requestParams);
        return Ok(productStock);
    }

    [HttpGet("StockQuantity/{warehouseId}/{productId}")]
    public async Task<IActionResult> StockQuantity(string companyId, string warehouseId, string productId)
    {
        var result = await _productStockService.GetStockQuantityByWarehouseAsync(companyId, warehouseId, productId);
        return Ok(result);
    }

    [HttpGet("LoteStockQuantity/{warehouseId}/{productLoteId}/{productId}")]
    public async Task<IActionResult> LoteStockQuantity(string companyId, string warehouseId, string productLoteId, string productId)
    {
        var result = await _productStockService.GetLoteStockQuantityByWarehouseAsync(companyId, warehouseId, productLoteId, productId);
        return Ok(result);
    }

}
