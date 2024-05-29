using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;
using Nebula.Common.Dto;

namespace Nebula.Modules.Inventory.Materiales;

public interface IMaterialService : ICrudOperationService<Material>
{
    Task<List<Material>> GetListAsync(string companyId, DateQuery query);
    Task<List<Material>> GetListByContactIdAsync(string companyId, DateQuery query, string contactId);
}

public class MaterialService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<Material>(mongoDatabase), IMaterialService
{
    public async Task<List<Material>> GetListAsync(string companyId, DateQuery query)
    {
        var filter = Builders<Material>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.Month, query.Month), filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(dbQuery).SortByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task<List<Material>> GetListByContactIdAsync(string companyId, DateQuery query, string contactId)
    {
        var builder = Builders<Material>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Month, query.Month), builder.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).SortByDescending(x => x.CreatedAt).ToListAsync();
    }
}
