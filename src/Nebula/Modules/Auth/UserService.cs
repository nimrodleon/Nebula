using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth;

public interface IUserService
{
    Task<List<User>> GetListAsync(string companyId, string query = "", int page = 1, int pageSize = 12);
    Task<long> GetTotalListAsync(string companyId, string query = "");
    Task<List<User>> GetUsersByUserIds(List<string> userIds);
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByUserNameAsync(string userName);
}

public class UserService : IUserService
{

    public async Task<List<User>> GetListAsync(string companyId, string query = "", int page = 1, int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;
        var builder = Builders<User>.Filter;
        var filter = builder.Eq(x => x.LocalDefault, companyId);

        if (!string.IsNullOrEmpty(query))
            filter = Builders<User>.Filter.Or(
                builder.Regex("FullName", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("UserName", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("Email", new BsonRegularExpression(query.ToUpper(), "i")));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<User>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();
    }

    public async Task<long> GetTotalListAsync(string companyId, string query = "")
    {
        var builder = Builders<User>.Filter;
        var filter = builder.Eq(x => x.LocalDefault, companyId);

        if (!string.IsNullOrEmpty(query))
            filter = builder.Regex("UserName", new BsonRegularExpression(query.ToUpper(), "i"));
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

    public async Task<User> GetByUserNameAsync(string userName)
    {
        var filter = Builders<User>.Filter.Eq(x => x.UserName, userName);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
