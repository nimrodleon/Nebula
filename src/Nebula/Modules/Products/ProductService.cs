using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products;

public interface IProductService : ICrudOperationService<Product>
{
    Task<List<Product>> GetListAsync(string companyId, string query = "", int limit = 24);
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
        return products;
    }

    public override async Task<Product> GetByIdAsync(string companyId, string id)
    {
        var product = await _collection.Find(x => x.CompanyId == companyId && x.Id == id).FirstOrDefaultAsync();
        return product;
    }

}
