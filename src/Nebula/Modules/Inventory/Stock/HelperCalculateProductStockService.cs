using Nebula.Database.Services.Common;
using Nebula.Modules.Inventory.Stock.Dto;
using Nebula.Modules.Inventory.Stock.Helpers;
using Nebula.Modules.Products;

namespace Nebula.Modules.Inventory.Stock;

public interface IHelperCalculateProductStockService
{
    Task<List<ProductStockInfoDto>> GetProductStockInfos(string productId);
}

public class HelperCalculateProductStockService : IHelperCalculateProductStockService
{
    private readonly ProductService _productService;
    private readonly ProductStockService _productStockService;
    private readonly WarehouseService _warehouseService;
    private readonly ProductLoteService _productLoteService;

    public HelperCalculateProductStockService(
        ProductService productService,
        ProductStockService productStockService,
        WarehouseService warehouseService,
        ProductLoteService productLoteService)
    {
        _productService = productService;
        _productStockService = productStockService;
        _warehouseService = warehouseService;
        _productLoteService = productLoteService;
    }

    /// <summary>
    /// Recupera la información de stock de un producto específico en diferentes almacenes y lotes
    /// </summary>
    /// <param name="productId">ID del producto a buscar</param>
    /// <returns></returns>
    public async Task<List<ProductStockInfoDto>> GetProductStockInfos(string productId)
    {
        var requestParams = new StockListRequestParams();
        requestParams.Product = await _productService.GetByIdAsync(productId);
        requestParams.Warehouses = await _warehouseService.GetAllAsync();
        requestParams.Lotes = await _productLoteService.GetLotesByProductId(requestParams.Product.Id);
        var warehouseIds = await _warehouseService.GetWarehouseIds();
        requestParams.Stocks = await _productStockService.GetProductStockListByWarehousesIdsAsync(warehouseIds, requestParams.Product.Id);
        var result = new HelperProductStockInfo(requestParams).GetStockList();
        return result;
    }
}
