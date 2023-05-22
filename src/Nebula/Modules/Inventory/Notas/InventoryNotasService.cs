using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;
using Nebula.Common.Dto;

namespace Nebula.Modules.Inventory.Notas;

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
