using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Data.Models;

namespace Nebula.Data.Services;

public class ContactService
{
    private readonly IMongoCollection<Contact> _collection;

    public ContactService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Contact>("Contacts");
    }

    public async Task<List<Contact>> GetListAsync(string? query)
    {
        var filter = Builders<Contact>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<Contact>.Filter.Regex("Name", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).Limit(25).ToListAsync();
    }

    public async Task<Contact> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Contact contact) =>
        await _collection.InsertOneAsync(contact);

    public async Task UpdateAsync(string id, Contact contact) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, contact);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
