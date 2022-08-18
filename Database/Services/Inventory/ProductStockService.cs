using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory;

public class ProductStockService : CrudOperationService<ProductStock>
{
    public ProductStockService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<ProductStock>> CreateAsync(List<ProductStock> obj)
    {
        await _collection.InsertManyAsync(obj);
        return obj;
    }

    public async Task RemoveAsync(string warehouseId, string productId)
    {
        var filter = Builders<ProductStock>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.WarehouseId, warehouseId),
            filter.Eq(x => x.ProductId, productId));

    }
}
