using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products;

public class ProductLoteService : CrudOperationService<ProductLote>
{
    private readonly ProductService _productService;

    public ProductLoteService(IOptions<DatabaseSettings> options, ProductService productService) : base(options)
    {
        _productService = productService;
    }

    public async Task<List<ProductLote>> GetLotesByExpirationDate(string id, string expirationDate)
    {
        var filter = Builders<ProductLote>.Filter.And(
            Builders<ProductLote>.Filter.Eq(x => x.ProductId, id),
            Builders<ProductLote>.Filter.Lte(x => x.ExpirationDate, expirationDate));
        var result = await _collection.Find(filter).ToListAsync();
        return result;
    }

    public async Task<List<ProductLote>> GetLotesByProductId(string productId)
    {
        var filter = Builders<ProductLote>.Filter.Eq(x => x.ProductId, productId);
        var result = await _collection.Find(filter).ToListAsync();
        return result;
    }

    public async Task<long> GetLoteCountByProductId(string productId)
    {
        var filter = Builders<ProductLote>.Filter.Eq(x => x.ProductId, productId);
        var count = await _collection.CountDocumentsAsync(filter);
        return count;
    }

    public async Task UpdateProductHasLote(string productId)
    {
        long countLote = await GetLoteCountByProductId(productId);
        await _productService.UpdateHasLote(productId, countLote > 0L);
    }

}
