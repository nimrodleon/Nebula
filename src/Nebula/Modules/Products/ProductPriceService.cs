using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products;

public interface IProductPriceService : ICrudOperationService<ProductPrices>
{
    Task<List<ProductPrices>> GetAsync(string productId);
    Task<long> GetPricesCountByProductId(string productId);
    Task UpdateProductHasPrices(string productId);
}

public class ProductPriceService : CrudOperationService<ProductPrices>, IProductPriceService
{
    private readonly IProductService _productService;

    public ProductPriceService(IOptions<DatabaseSettings> options,
        IProductService productService) : base(options)
    {
        _productService = productService;
    }

    public async Task<List<ProductPrices>> GetAsync(string productId)
    {
        var builder = Builders<ProductPrices>.Filter;
        var filter = builder.Eq(x => x.ProductId, productId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<long> GetPricesCountByProductId(string productId)
    {
        var filter = Builders<ProductPrices>.Filter.Eq(x => x.ProductId, productId);
        var count = await _collection.CountDocumentsAsync(filter);
        return count;
    }

    public async Task UpdateProductHasPrices(string productId)
    {
        long countPrices = await GetPricesCountByProductId(productId);
        await _productService.UpdateHasPrices(productId, countPrices > 0L);
    }

}
