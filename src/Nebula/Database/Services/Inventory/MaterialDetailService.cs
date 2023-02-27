using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory;

public class MaterialDetailService : CrudOperationService<MaterialDetail>
{
    public MaterialDetailService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<MaterialDetail>> GetListAsync(string id)
    {
        var filter = Builders<MaterialDetail>.Filter;
        var query = filter.Eq(x => x.MaterialId, id);
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string id) =>
        await _collection.CountDocumentsAsync(x => x.MaterialId == id);
}
