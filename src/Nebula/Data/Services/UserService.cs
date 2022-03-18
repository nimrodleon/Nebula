using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Data.Models;

namespace Nebula.Data.Services;

public class UserService
{
    private readonly IMongoCollection<User> _collection;

    public UserService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<User>("Users");
    }

    public async Task<List<User>> GetListAsync(string? query, int limit = 25)
    {
        var filter = Builders<User>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<User>.Filter.Regex("UserName", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }

    public async Task<User> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) =>
        await _collection.InsertOneAsync(user);

    public async Task UpdateAsync(string id, User user) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, user);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
