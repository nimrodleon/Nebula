using Nebula.Modules.Account;
using Nebula.Modules.Inventory.Stock.Dto;
using Nebula.Modules.Inventory.Stock.Helpers;
using Nebula.Modules.Products;

namespace Nebula.Modules.Inventory.Stock;

public interface IHelperCalculateProductStockService
{
    Task<List<ProductStockInfoDto>> GetProductStockInfos(string companyId, string productId);
}

public class HelperCalculateProductStockService(
    IProductService productService,
    IProductStockService productStockService,
    IWarehouseService warehouseService)
    : IHelperCalculateProductStockService
{
    /// <summary>
    /// Recupera la información de stock de un producto específico en diferentes almacenes y lotes
    /// </summary>
    /// <param name="productId">ID del producto a buscar</param>
    /// <returns></returns>
    public async Task<List<ProductStockInfoDto>> GetProductStockInfos(string companyId, string productId)
    {
        var requestParams = new StockListRequestParams();
        requestParams.Product = await productService.GetByIdAsync(companyId, productId);
        requestParams.Warehouses = await warehouseService.GetAllAsync(companyId);
        var warehouseArrId = new List<string>();
        requestParams.Warehouses.ForEach(item => warehouseArrId.Add(item.Id));
        requestParams.Stocks = await productStockService.GetProductStocksByWarehousesIdsAsync(companyId, warehouseArrId, requestParams.Product.Id);
        var result = new HelperProductStockInfo(requestParams).GetStockInfo();
        return result;
    }
}
