using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Data.Models;

namespace Nebula.Data.Services;

public class CrudOperationService<T> where T : class, Generic
{
    protected readonly IMongoCollection<T> _collection;

    public CrudOperationService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
    }

    public async Task<List<T>> GetAsync(string field, string? query, int limit = 25)
    {
        var filter = Builders<T>.Filter.Empty;
        if (!string.IsNullOrWhiteSpace(query))
            filter = Builders<T>.Filter.Regex(field, new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }

    public async Task<T> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(T obj) =>
        await _collection.InsertOneAsync(obj);

    public async Task UpdateAsync(string id, T obj) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, obj);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
