using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products;

public interface IProductPriceService : ICrudOperationService<ProductPrices>
{
    Task<List<ProductPrices>> GetAsync(string companyId, string productId);
    Task<long> GetPricesCountByProductId(string companyId, string productId);
    Task UpdateProductHasPrices(string companyId, string productId);
}

public class ProductPriceService : CrudOperationService<ProductPrices>, IProductPriceService
{
    private readonly IProductService _productService;

    public ProductPriceService(MongoDatabaseService mongoDatabase,
        IProductService productService) : base(mongoDatabase)
    {
        _productService = productService;
    }

    public async Task<List<ProductPrices>> GetAsync(string companyId, string productId)
    {
        var builder = Builders<ProductPrices>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.ProductId, productId));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<long> GetPricesCountByProductId(string companyId, string productId)
    {
        var builder = Builders<ProductPrices>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId), builder.Eq(x => x.ProductId, productId));
        var count = await _collection.CountDocumentsAsync(filter);
        return count;
    }

    public async Task UpdateProductHasPrices(string companyId, string productId)
    {
        long countPrices = await GetPricesCountByProductId(companyId, productId);
        await _productService.UpdateHasPrices(companyId, productId, countPrices > 0L);
    }

}
