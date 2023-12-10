using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Materiales;

public interface IMaterialDetailService : ICrudOperationService<MaterialDetail>
{
    Task<List<MaterialDetail>> GetListAsync(string companyId, string materialId);
    Task<long> CountDocumentsAsync(string companyId, string id);
}

public class MaterialDetailService : CrudOperationService<MaterialDetail>, IMaterialDetailService
{
    public MaterialDetailService(MongoDatabaseService mongoDatabase) : base(mongoDatabase) { }

    public async Task<List<MaterialDetail>> GetListAsync(string companyId, string materialId)
    {
        var filter = Builders<MaterialDetail>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId), filter.Eq(x => x.MaterialId, materialId));
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string companyId, string id) =>
        await _collection.CountDocumentsAsync(x => x.CompanyId == companyId && x.MaterialId == id);
}
