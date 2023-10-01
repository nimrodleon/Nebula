using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth;

namespace Nebula.Modules.Account;

public interface ICompanyService : ICrudOperationService<Company>
{
    Task<List<Company>> GetCompaniesByIds(string[] companyIds);
    Task<List<Company>> GetCompaniesByUserIdAsync(string userId);
}

public class CompanyService : CrudOperationService<Company>, ICompanyService
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public CompanyService(MongoDatabaseService mongoDatabase,
        IUserAuthenticationService userAuthenticationService) : base(mongoDatabase)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    public async Task<List<Company>> GetCompaniesByIds(string[] companyIds)
    {
        var filter = Builders<Company>.Filter.In(x => x.Id, companyIds);
        return await _collection.Find(filter).ToListAsync();
    }

    public override async Task<List<Company>> GetAsync(string field, string? query, int limit = 25)
    {
        var userId = _userAuthenticationService.GetUserId();
        var filter = Builders<Company>.Filter.Eq(x => x.UserId, userId);
        if (!string.IsNullOrWhiteSpace(query))
        {
            var queryFilter = Builders<Company>.Filter.Regex(field, new BsonRegularExpression(query.ToUpper(), "i"));
            filter = Builders<Company>.Filter.And(filter, queryFilter);

        }
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }

    public async Task<List<Company>> GetCompaniesByUserIdAsync(string userId)
    {
        var filter = Builders<Company>.Filter.Eq(x => x.UserId, userId.Trim());
        return await _collection.Find(filter).ToListAsync();
    }

    public override async Task<Company> CreateAsync(Company obj)
    {
        obj.UserId = _userAuthenticationService.GetUserId();
        return await base.CreateAsync(obj);
    }

    public override async Task<Company> UpdateAsync(string id, Company obj)
    {
        obj.UserId = _userAuthenticationService.GetUserId();
        return await base.UpdateAsync(id, obj);
    }
}
