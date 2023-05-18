using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory;

public class InventoryNotasDetailService : CrudOperationService<InventoryNotasDetail>
{
    public InventoryNotasDetailService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<InventoryNotasDetail>> GetListAsync(string id)
    {
        var filter = Builders<InventoryNotasDetail>.Filter;
        var query = filter.Eq(x => x.InventoryNotasId, id);
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string id) =>
        await _collection.CountDocumentsAsync(x => x.InventoryNotasId == id);
}
