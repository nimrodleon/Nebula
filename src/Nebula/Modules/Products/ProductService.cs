using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products;

public interface IProductService : ICrudOperationService<Product>
{
    Task<List<Product>> GetListAsync(string companyId, string query = "", int limit = 10);
    Task<List<Product>> GetProductosAsync(string companyId, string query = "", int page = 1, int pageSize = 12);
    Task<long> GetTotalProductosAsync(string companyId, string query = "");
}

public class ProductService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<Product>(mongoDatabase), IProductService
{
    public async Task<List<Product>> GetListAsync(string companyId, string query = "", int limit = 10)
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

    public async Task<List<Product>> GetProductosAsync(string companyId, string query = "", int page = 1, int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;

        var builder = Builders<Product>.Filter;
        var filter = builder.Eq(x => x.CompanyId, companyId);

        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Or(
                builder.Regex("Barcode", new BsonRegularExpression(query, "i")),
                builder.Regex("Description", new BsonRegularExpression(query.ToUpper(), "i"))
            );
        }

        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<Product>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();
    }

    public async Task<long> GetTotalProductosAsync(string companyId, string query = "")
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Eq(x => x.CompanyId, companyId);

        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Or(
                builder.Regex("Barcode", new BsonRegularExpression(query, "i")),
                builder.Regex("Description", new BsonRegularExpression(query.ToUpper(), "i"))
            );
        }

        return await _collection.Find(filter).CountDocumentsAsync();
    }

}
