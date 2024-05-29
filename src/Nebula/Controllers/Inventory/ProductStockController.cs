using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nebula.Modules.Auth;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Stock.Dto;

namespace Nebula.Controllers.Inventory;

[Authorize]
[CustomerAuthorize(UserRole = UserRole.User)]
[Route("api/inventory/[controller]")]
[ApiController]
public class ProductStockController(
    IUserAuthenticationService userAuthenticationService,
    IProductStockService productStockService,
    IHelperCalculateProductStockService helperCalculateProductStockService)
    : ControllerBase
{
    private readonly string _companyId = userAuthenticationService.GetDefaultCompanyId();

    [HttpGet("GetStockInfos/{productId}")]
    public async Task<IActionResult> GetStockInfos(string productId)
    {
        var responseData = await helperCalculateProductStockService.GetProductStockInfos(_companyId, productId);
        return Ok(responseData);
    }

    [HttpPost("ChangeQuantity")]
    public async Task<IActionResult> ChangeQuantity([FromBody] ChangeQuantityStockRequestParams requestParams)
    {
        var productStock = await productStockService.ChangeQuantity(_companyId, requestParams);
        return Ok(productStock);
    }

    [HttpGet("StockQuantity/{warehouseId}/{productId}")]
    public async Task<IActionResult> StockQuantity(string warehouseId, string productId)
    {
        var result = await productStockService.GetStockQuantityAsync(_companyId, warehouseId, productId);
        return Ok(result);
    }
}
