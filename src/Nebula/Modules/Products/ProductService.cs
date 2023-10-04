using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products;

public interface IProductService : ICrudOperationService<Product>
{
    Task<List<Product>> GetListAsync(string companyId, string query = "", int limit = 24);
    Task<bool> UpdateHasLote(string companyId, string productId, bool hasLote);
    Task<bool> UpdateHasPrices(string companyId, string productId, bool hasPrices);
}

public class ProductService : CrudOperationService<Product>, IProductService
{
    public ProductService(MongoDatabaseService mongoDatabase) : base(mongoDatabase) { }

    public async Task<List<Product>> GetListAsync(string companyId, string query = "", int limit = 25)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.CompanyId, companyId);

        if (!string.IsNullOrEmpty(query))
        {
            var textSearchFilter = Builders<Product>.Filter.Or(
                Builders<Product>.Filter.Eq("Barcode", query),
                Builders<Product>.Filter.Regex("Description", new BsonRegularExpression(query.ToUpper(), "i"))
            );
            filter = Builders<Product>.Filter.And(filter, textSearchFilter);
        }

        var products = await _collection.Find(filter).Limit(limit).ToListAsync();
        //if (!configuration.ModLotes) products.ForEach(item => item.HasLotes = false);
        return products;
    }

    public override async Task<Product> GetByIdAsync(string companyId, string id)
    {
        var product = await _collection.Find(x => x.CompanyId == companyId && x.Id == id).FirstOrDefaultAsync();
        //var configuration = await _configurationService.GetAsync();
        //if (!configuration.ModLotes) product.HasLotes = false;
        return product;
    }

    public async Task<bool> UpdateHasLote(string companyId, string productId, bool hasLote)
    {
        var filter = Builders<Product>.Filter.And(
            Builders<Product>.Filter.Eq(x => x.CompanyId, companyId),
            Builders<Product>.Filter.Eq(x => x.Id, productId));
        var update = Builders<Product>.Update.Set(x => x.HasLotes, hasLote);
        var result = await _collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount == 1;
    }

    public async Task<bool> UpdateHasPrices(string companyId, string productId, bool hasPrices)
    {
        var filter = Builders<Product>.Filter.And(
            Builders<Product>.Filter.Eq(x => x.CompanyId, companyId),
            Builders<Product>.Filter.Eq(x => x.Id, productId));
        var update = Builders<Product>.Update.Set(x => x.HasPrices, hasPrices);
        var result = await _collection.UpdateOneAsync(filter, update);
        return result.ModifiedCount == 1;
    }

}
