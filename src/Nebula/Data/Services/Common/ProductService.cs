using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Data.Models.Common;

namespace Nebula.Data.Services.Common;

public class ProductService
{
    private readonly IMongoCollection<Product> _collection;

    public ProductService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Product>("Products");
    }

    public async Task<List<Product>> GetListAsync(string? query, int limit = 25)
    {
        var filter = Builders<Product>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<Product>.Filter.Regex("Description", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }

    public async Task<Product> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Product product) =>
        await _collection.InsertOneAsync(product);

    public async Task UpdateAsync(string id, Product product) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, product);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
