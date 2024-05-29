using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;
using Nebula.Common.Dto;

namespace Nebula.Modules.Inventory.Notas;

public interface IInventoryNotasService : ICrudOperationService<InventoryNotas>
{
    Task<List<InventoryNotas>> GetListAsync(string companyId, DateQuery query);
}

public class InventoryNotasService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<InventoryNotas>(mongoDatabase), IInventoryNotasService
{
    public async Task<List<InventoryNotas>> GetListAsync(string companyId, DateQuery query)
    {
        var filter = Builders<InventoryNotas>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.Month, query.Month), filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(dbQuery).SortByDescending(x => x.CreatedAt).ToListAsync();
    }
}
