using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Notas;

public interface IInventoryNotasDetailService : ICrudOperationService<InventoryNotasDetail>
{
    Task<List<InventoryNotasDetail>> GetListAsync(string companyId, string id);
    Task<long> CountDocumentsAsync(string companyId, string id);
}

public class InventoryNotasDetailService : CrudOperationService<InventoryNotasDetail>, IInventoryNotasDetailService
{
    public InventoryNotasDetailService(MongoDatabaseService mongoDatabase) : base(mongoDatabase) { }

    public async Task<List<InventoryNotasDetail>> GetListAsync(string companyId, string id)
    {
        var filter = Builders<InventoryNotasDetail>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId), filter.Eq(x => x.InventoryNotasId, id));
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string companyId, string id) =>
        await _collection.CountDocumentsAsync(x => x.CompanyId == companyId && x.InventoryNotasId == id);
}
