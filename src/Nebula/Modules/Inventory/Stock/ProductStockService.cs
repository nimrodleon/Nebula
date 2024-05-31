using MongoDB.Driver;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Common;
using Nebula.Modules.Inventory.Dto;

namespace Nebula.Modules.Inventory.Stock;

public interface IProductStockService : ICrudOperationService<ProductStock>
{
    Task<List<ProductStock>> CreateManyAsync(List<ProductStock> obj);
    Task<DeleteResult> DeleteAllStockAsync(string companyId, string warehouseId, string productId);
    Task<ProductStock> ChangeQuantity(string companyId, ChangeQuantityStockRequestParams requestParams);
    Task<decimal> GetStockQuantityAsync(string companyId, string warehouseId, string productId);
    Task<List<ProductStock>> GetProductStocksAsync(string companyId, string warehouseId, string productId);
    Task<List<ProductStock>> GetProductStocksByWarehousesIdsAsync(string companyId, List<string> warehouseArrId, string productId);
    Task<DeleteResult> DeleteProductStockByProductIdsAsync(string companyId, string warehouseId, List<string> productArrId);
    Task<List<ProductStock>> GetProductStockByProductIdsAsync(string companyId, string warehouseId, List<string> productArrId);
}

public class ProductStockService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<ProductStock>(mongoDatabase), IProductStockService
{
    public async Task<List<ProductStock>> CreateManyAsync(List<ProductStock> obj)
    {
        await _collection.InsertManyAsync(obj);
        return obj;
    }

    public async Task<DeleteResult> DeleteAllStockAsync(string companyId, string warehouseId, string productId)
    {
        var filter = Builders<ProductStock>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.WarehouseId, warehouseId), filter.Eq(x => x.ProductId, productId));
        return await _collection.DeleteManyAsync(dbQuery);
    }

    public async Task<ProductStock> ChangeQuantity(string companyId, ChangeQuantityStockRequestParams requestParams)
    {
        await DeleteAllStockAsync(companyId, requestParams.WarehouseId, requestParams.ProductId);
        var productStock = new ProductStock()
        {
            Id = string.Empty,
            CompanyId = companyId.Trim(),
            WarehouseId = requestParams.WarehouseId,
            ProductId = requestParams.ProductId,
            TransactionType = TransactionType.ENTRADA,
            Quantity = requestParams.Quantity,
        };
        productStock = await InsertOneAsync(productStock);
        return productStock;
    }

    private decimal CalculateNetStockQuantity(List<ProductStock> productStocks)
    {
        var totalEntrada = productStocks.Where(x => x.TransactionType == TransactionType.ENTRADA).Sum(x => x.Quantity);
        var totalSalida = productStocks.Where(x => x.TransactionType == TransactionType.SALIDA).Sum(x => x.Quantity);
        return totalEntrada - totalSalida;
    }

    public async Task<decimal> GetStockQuantityAsync(string companyId, string warehouseId, string productId)
    {
        var productStocks = await GetProductStocksAsync(companyId, warehouseId, productId);
        return CalculateNetStockQuantity(productStocks);
    }

    public async Task<List<ProductStock>> GetProductStocksAsync(string companyId, string warehouseId, string productId)
    {
        var builder = Builders<ProductStock>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.WarehouseId, warehouseId),
            builder.Eq(x => x.ProductId, productId));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<ProductStock>> GetProductStocksByWarehousesIdsAsync(string companyId, List<string> warehouseArrId, string productId)
    {
        var builder = Builders<ProductStock>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.ProductId, productId), builder.In("WarehouseId", warehouseArrId));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<DeleteResult> DeleteProductStockByProductIdsAsync(string companyId, string warehouseId, List<string> productArrId)
    {
        var builder = Builders<ProductStock>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.WarehouseId, warehouseId), builder.In("ProductId", productArrId));
        return await _collection.DeleteManyAsync(filter);
    }

    public async Task<List<ProductStock>> GetProductStockByProductIdsAsync(string companyId, string warehouseId, List<string> productArrId)
    {
        var builder = Builders<ProductStock>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.WarehouseId, warehouseId), builder.In("ProductId", productArrId));
        return await _collection.Find(filter).ToListAsync();
    }

}
