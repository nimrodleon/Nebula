using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Nebula.Common;

public interface ICacheService
{
    Task SetUserAuthAsync(User user);
    Task<User?> GetUserAuthAsync(string userId);
    Task SetUserAuthCompaniesAsync(string userId, List<Company> companies);
}

public class CacheService : ICacheService
{
    private readonly IDatabase _database;

    public CacheService(IDatabase database)
    {
        _database = database;
    }

    public async Task SetUserAuthAsync(User user)
    {
        var serializedUser = JsonSerializer.Serialize(user);
        await _database.StringSetAsync($"nebula_user_{user.Id}", serializedUser);
        await _database.KeyExpireAsync($"nebula_user_{user.Id}", TimeSpan.FromMinutes(1000));
    }

    public async Task<User?> GetUserAuthAsync(string userId)
    {
        var serializedUser = await _database.StringGetAsync($"nebula_user_{userId}");
        if (serializedUser.HasValue && !serializedUser.IsNull)
        {
            var user = JsonSerializer.Deserialize<User>(serializedUser,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return user;
        }
        return null;
    }

    public async Task SetUserAuthCompaniesAsync(string userId, List<Company> companies)
    {
        var serializedCompanies = JsonSerializer.Serialize(companies);
        await _database.StringSetAsync($"nebula_user_companies_{userId}", serializedCompanies);
        await _database.KeyExpireAsync($"nebula_user_companies_{userId}", TimeSpan.FromMinutes(1000));
    }

    public async Task<List<Company>> GetUserAuthCompaniesAsync(string userId)
    {
        var serializedCompanies = await _database.StringGetAsync($"nebula_user_companies_{userId}");

        if (serializedCompanies.HasValue && !serializedCompanies.IsNull)
        {
            var companies = JsonSerializer.Deserialize<List<Company>>(serializedCompanies,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return companies;
        }

        return new List<Company>();
    }

}
