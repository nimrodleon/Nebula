using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;
using Nebula.Common.Dto;

namespace Nebula.Modules.Inventory.Materiales;

public interface IMaterialService : ICrudOperationService<Material>
{
    Task<List<Material>> GetListAsync(DateQuery query);
    Task<List<Material>> GetListByContactIdAsync(DateQuery query, string contactId);
}

public class MaterialService : CrudOperationService<Material>, IMaterialService
{
    public MaterialService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<Material>> GetListAsync(DateQuery query)
    {
        var filter = Builders<Material>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.Month, query.Month),
            filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(dbQuery).SortByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task<List<Material>> GetListByContactIdAsync(DateQuery query, string contactId)
    {
        var builder = Builders<Material>.Filter;
        var filter = builder.And(builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Month, query.Month), builder.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).SortByDescending(x => x.CreatedAt).ToListAsync();
    }
}
