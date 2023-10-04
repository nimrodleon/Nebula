using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products;

public interface IProductLoteService : ICrudOperationService<ProductLote>
{
    Task<List<ProductLote>> GetLotesByExpirationDate(string companyId, string id, string expirationDate);
    Task<List<ProductLote>> GetLotesByProductId(string companyId, string productId);
    Task<long> GetLoteCountByProductId(string companyId, string productId);
    Task UpdateProductHasLote(string companyId, string productId);
}

public class ProductLoteService : CrudOperationService<ProductLote>, IProductLoteService
{
    private readonly IProductService _productService;

    public ProductLoteService(MongoDatabaseService mongoDatabase, IProductService productService) : base(mongoDatabase)
    {
        _productService = productService;
    }

    public async Task<List<ProductLote>> GetLotesByExpirationDate(string companyId, string id, string expirationDate)
    {
        var filter = Builders<ProductLote>.Filter.And(
            Builders<ProductLote>.Filter.Eq(x =>x.CompanyId, companyId),
            Builders<ProductLote>.Filter.Eq(x => x.ProductId, id),
            Builders<ProductLote>.Filter.Lte(x => x.ExpirationDate, expirationDate));
        var result = await _collection.Find(filter).ToListAsync();
        return result;
    }

    public async Task<List<ProductLote>> GetLotesByProductId(string companyId, string productId)
    {
        var filter = Builders<ProductLote>.Filter.And(
            Builders<ProductLote>.Filter.Eq(x => x.CompanyId, companyId),   
            Builders<ProductLote>.Filter.Eq(x => x.ProductId, productId));
        var result = await _collection.Find(filter).ToListAsync();
        return result;
    }

    public async Task<long> GetLoteCountByProductId(string companyId, string productId)
    {
        var filter = Builders<ProductLote>.Filter.And(
            Builders<ProductLote>.Filter.Eq(x => x.CompanyId, companyId),
            Builders<ProductLote>.Filter.Eq(x => x.ProductId, productId));
        var count = await _collection.CountDocumentsAsync(filter);
        return count;
    }

    public async Task UpdateProductHasLote(string companyId, string productId)
    {
        long countLote = await GetLoteCountByProductId(companyId, productId);
        await _productService.UpdateHasLote(companyId, productId, countLote > 0L);
    }

}
