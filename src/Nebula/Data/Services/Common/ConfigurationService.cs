using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data.Models.Common;

namespace Nebula.Data.Services.Common;

public class ConfigurationService
{
    private readonly IMongoCollection<Configuration> _collection;

    public ConfigurationService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Configuration>("Configuration");
    }

    public async Task<Configuration> GetAsync() =>
        await _collection.Find(x => x.Id == "DEFAULT").FirstOrDefaultAsync();

    public async Task CreateAsync() =>
        await _collection.InsertOneAsync(new Configuration());

    public async Task UpdateAsync(Configuration configuration) =>
        await _collection.ReplaceOneAsync(x => x.Id == "DEFAULT", configuration);
}
