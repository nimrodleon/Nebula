using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Data.Models;

namespace Nebula.Data.Services;

public class WarehouseService
{
    private readonly IMongoCollection<Warehouse> _collection;

    public WarehouseService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Warehouse>("Warehouses");
    }

    public async Task<List<Warehouse>> GetListAsync(string? query)
    {
        var filter = Builders<Warehouse>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<Warehouse>.Filter.Regex("Name", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<Warehouse> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Warehouse warehouse) =>
        await _collection.InsertOneAsync(warehouse);

    public async Task UpdateAsync(string id, Warehouse warehouse) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, warehouse);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
