using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Dto.Common;

namespace Nebula.Database.Services.Inventory;

public class InventoryNotasService : CrudOperationService<InventoryNotas>
{
    public InventoryNotasService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<InventoryNotas>> GetListAsync(DateQuery query)
    {
        var filter = Builders<InventoryNotas>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.Month, query.Month),
            filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(dbQuery).SortByDescending(x => x.CreatedAt).ToListAsync();
    }
}