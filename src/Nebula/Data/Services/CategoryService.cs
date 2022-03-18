using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data.Models;

namespace Nebula.Data.Services;

public class CategoryService
{
    private readonly IMongoCollection<Category> _collection;

    public CategoryService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Category>("Categories");
    }

    public async Task<List<Category>> GetListAsync(string query) =>
        await _collection.Find(x => x.Name.Contains(query)).ToListAsync();

    public async Task<Category?> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Category category) =>
        await _collection.InsertOneAsync(category);

    public async Task UpdateAsync(string id, Category category) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, category);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
