using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Database.Models.Common;

namespace Nebula.Database.Services.Common;

public class ProductService : CrudOperationService<Product>
{
    public ProductService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<Product>> GetListAsync(string? query, int limit = 24)
    {
        var filter = Builders<Product>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<Product>.Filter.Or(Builders<Product>.Filter.Eq("Barcode", query),
                Builders<Product>.Filter.Regex("Description", new BsonRegularExpression(query.ToUpper(), "i")));
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }
}
