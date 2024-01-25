using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth;

namespace Nebula.Modules.Account;

public interface ICompanyService : ICrudOperationService<Company>
{
    Task<Company> GetCompany(string companyId);
    Task<List<Company>> GetCompaniesByIds(string[] companyIds);
    Task<List<Company>> GetCompaniesByUserIdAsync(string userId);
    Task<long> GetTotalCompaniesAsync(string query = "");
    Task<List<Company>> GetCompaniesAsync(string query = "", int page = 1, int pageSize = 12);
}

public class CompanyService : CrudOperationService<Company>, ICompanyService
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public CompanyService(MongoDatabaseService mongoDatabase,
        IUserAuthenticationService userAuthenticationService) : base(mongoDatabase)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    public async Task<List<Company>> GetCompaniesAsync(string query = "", int page = 1, int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;

        var builder = Builders<Company>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Or(
                builder.Regex("Ruc", new BsonRegularExpression(query, "i")),
                builder.Regex("RznSocial", new BsonRegularExpression(query.ToUpper(), "i"))
            );
        }

        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<Company>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();
    }

    public async Task<long> GetTotalCompaniesAsync(string query = "")
    {
        var builder = Builders<Company>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Or(
                builder.Regex("Ruc", new BsonRegularExpression(query, "i")),
                builder.Regex("RznSocial", new BsonRegularExpression(query.ToUpper(), "i"))
            );
        }

        return await _collection.Find(filter).CountDocumentsAsync();
    }

    public async Task<Company> GetCompany(string companyId)
    {
        var filter = Builders<Company>.Filter.Eq(x => x.Id, companyId.Trim());
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<List<Company>> GetCompaniesByIds(string[] companyIds)
    {
        var filter = Builders<Company>.Filter.In(x => x.Id, companyIds);
        return await _collection.Find(filter).ToListAsync();
    }

    public override async Task<List<Company>> GetAsync(string field, string query = "", int limit = 25)
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
