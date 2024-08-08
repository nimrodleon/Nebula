using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth;

public interface IUserService : ICrudOperationService<User>
{
    Task<List<User>> GetListAsync(string query = "", int page = 1, int pageSize = 12);
    Task<long> GetTotalListAsync(string query = "");
    Task<List<User>> GetUsersByUserIds(List<string> userIds);
    Task<User> GetByEmailAsync(string email);
}

public class UserService : CrudOperationService<User>, IUserService
{
    public UserService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
        var indexKeys = Builders<User>.IndexKeys.Ascending(x => x.Email);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var model = new CreateIndexModel<User>(indexKeys, indexOptions);
        _collection.Indexes.CreateOne(model);
    }

    public async Task<List<User>> GetListAsync(string query = "", int page = 1, int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;
        var filter = Builders<User>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<User>.Filter.Or(
                Builders<User>.Filter.Regex("UserName", new BsonRegularExpression(query.ToUpper(), "i")),
                Builders<User>.Filter.Regex("Email", new BsonRegularExpression(query.ToUpper(), "i")));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<User>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();
    }

    public async Task<long> GetTotalListAsync(string query = "")
    {
        var filter = Builders<User>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<User>.Filter.Regex("UserName", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).CountDocumentsAsync();
    }

    public async Task<List<User>> GetUsersByUserIds(List<string> userIds)
    {
        var filter = Builders<User>.Filter.In(x => x.Id, userIds);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Email, email);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
