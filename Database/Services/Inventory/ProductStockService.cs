using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Common;
using Nebula.Database.Models.Inventory;
using Nebula.Database.ViewModels.Inventory;

namespace Nebula.Database.Services.Inventory;

public class ProductStockService : CrudOperationService<ProductStock>
{
    private readonly CrudOperationService<Warehouse> _warehouseService;

    public ProductStockService(IOptions<DatabaseSettings> options,
        CrudOperationService<Warehouse> warehouseService) : base(options)
    {
        _warehouseService = warehouseService;
    }

    public async Task<List<ProductStock>> CreateAsync(List<ProductStock> obj)
    {
        await _collection.InsertManyAsync(obj);
        return obj;
    }

    public async Task<DeleteResult> RemoveAsync(string warehouseId, string productId)
    {
        var filter = Builders<ProductStock>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.WarehouseId, warehouseId),
            filter.Eq(x => x.ProductId, productId));
        return await _collection.DeleteManyAsync(dbQuery);
    }

    public async Task<List<ProductStockReport>> GetReport(string id)
    {
        var warehouses = await _warehouseService.GetAsync("Name", string.Empty);
        var filter = Builders<ProductStock>.Filter;
        var warehouseIds = new List<string>();
        warehouses.ForEach(item => warehouseIds.Add(item.Id));
        var dbQuery = filter.And(filter.Eq(x => x.ProductId, id), filter.In("WarehouseId", warehouseIds));
        var productStocks = await _collection.Find(dbQuery).ToListAsync();
        var productStockReports = new List<ProductStockReport>();
        warehouses.ForEach(item =>
        {
            var products = productStocks.Where(x => x.WarehouseId == item.Id).ToList();
            var entrada = products.Where(x => x.Type == InventoryType.ENTRADA).Sum(x => x.Quantity);
            var salida = products.Where(x => x.Type == InventoryType.SALIDA).Sum(x => x.Quantity);
            productStockReports.Add(new ProductStockReport
            {
                WarehouseId = item.Id,
                WarehouseName = item.Name,
                Quantity = entrada - salida,
            });
        });
        return productStockReports;
    }
}
