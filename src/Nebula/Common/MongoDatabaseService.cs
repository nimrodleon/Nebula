using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Nebula.Common;

public class MongoDatabaseService
{
    private readonly IMongoDatabase _database;

    public MongoDatabaseService(IOptions<DatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }
}
