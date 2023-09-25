using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Configurations;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products;

public interface IProductService : ICrudOperationService<Product>
{
    Task<List<Product>> GetListAsync(string? query, int limit = 24);
    Task<bool> UpdateHasLote(string productId, bool hasLote);
    Task<bool> UpdateHasPrices(string productId, bool hasPrices);
}

public class ProductService : CrudOperationService<Product>, IProductService
{
    private readonly IConfigurationService _configurationService;

    public ProductService(MongoDatabaseService mongoDatabase,
        IConfigurationService configurationService) : base(mongoDatabase)
    {
        _configurationService = configurationService;
    }

    public async Task<List<Product>> GetListAsync(string? query, int limit = 25)
    {
        var filter = Builders<Product>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<Product>.Filter.Or(Builders<Product>.Filter.Eq("Barcode", query),
                Builders<Product>.Filter.Regex("Description", new BsonRegularExpression(query.ToUpper(), "i")));
        var products = await _collection.Find(filter).Limit(limit).ToListAsync();
        var configuration = await _configurationService.GetAsync();
        if (!configuration.ModLotes) products.ForEach(item => item.HasLotes = false);
        return products;
    }

    public override async Task<Product> GetByIdAsync(string id)
    {
        var product = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        var configuration = await _configurationService.GetAsync();
        if (!configuration.ModLotes) product.HasLotes = false;
        return product;
    }

    public async Task<bool> UpdateHasLote(string productId, bool hasLote)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Id, productId);
        var update = Builders<Product>.Update.Set(x => x.HasLotes, hasLote);
        var result = await _collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount == 1;
    }

    public async Task<bool> UpdateHasPrices(string productId, bool hasPrices)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Id, productId);
        var update = Builders<Product>.Update.Set(x => x.HasPrices, hasPrices);
        var result = await _collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount == 1;
    }

}
