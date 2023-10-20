using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth;
using Nebula.Modules.Auth.Dto;
using Nebula.Modules.Auth.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Nebula.Common;

public interface ICacheAuthService
{
    Task SetUserAuthAsync(User user);
    Task<User?> GetUserAuthAsync(string userId);
    Task SetUserAuthCompaniesAsync(string userId, List<Company> companies);
    Task<List<Company>> GetUserAuthCompaniesAsync(string userId);
    Task<Company> GetCompanyByIdAsync(string companyId);
    Task SetUserAuthCompanyRolesAsync(string userId, List<UserCompanyRole> companyRoles);
    Task<List<UserCompanyRole>> GetUserAuthCompanyRolesAsync(string userId);
    Task RemoveUserAuthCompanyRolesAsync(string userId);
}

public class CacheAuthService : ICacheAuthService
{
    private readonly IDatabase _database;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public CacheAuthService(IDatabase database, IUserAuthenticationService userAuthenticationService)
    {
        _database = database;
        _userAuthenticationService = userAuthenticationService;
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

    public async Task<Company> GetCompanyByIdAsync(string companyId)
    {
        string userId = _userAuthenticationService.GetUserId();
        var companies = await GetUserAuthCompaniesAsync(userId);
        var company = companies.FirstOrDefault(x => x.Id == companyId.Trim()) ?? new Company();
        return company;
    }

    public async Task SetUserAuthCompanyRolesAsync(string userId, List<UserCompanyRole> companyRoles)
    {
        var serializedCompanyRoles = JsonSerializer.Serialize(companyRoles);
        await _database.StringSetAsync($"nebula_user_company_roles_{userId}", serializedCompanyRoles);
        await _database.KeyExpireAsync($"nebula_user_company_roles_{userId}", TimeSpan.FromMinutes(1000));
    }

    public async Task<List<UserCompanyRole>> GetUserAuthCompanyRolesAsync(string userId)
    {
        var serializedCompanyRoles = await _database.StringGetAsync($"nebula_user_company_roles_{userId}");

        if (serializedCompanyRoles.HasValue && !serializedCompanyRoles.IsNull)
        {
            var companyRoles = JsonSerializer.Deserialize<List<UserCompanyRole>>(serializedCompanyRoles,
                                  new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return companyRoles;
        }

        return new List<UserCompanyRole>();
    }

    public async Task RemoveUserAuthCompanyRolesAsync(string userId)
    {
        await _database.KeyDeleteAsync($"nebula_user_company_roles_{userId}");
    }

}
