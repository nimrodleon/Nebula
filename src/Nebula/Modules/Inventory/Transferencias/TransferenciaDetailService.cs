using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Transferencias;

public interface ITransferenciaDetailService : ICrudOperationService<TransferenciaDetail>
{
    Task<List<TransferenciaDetail>> GetListAsync(string companyId, string id);
    Task<long> CountDocumentsAsync(string companyId, string id);
    Task<DeleteResult> DeleteManyAsync(string companyId, string transferenciaId);
}

public class TransferenciaDetailService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<TransferenciaDetail>(mongoDatabase), ITransferenciaDetailService
{
    public async Task<List<TransferenciaDetail>> GetListAsync(string companyId, string transferenciaId)
    {
        var filter = Builders<TransferenciaDetail>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId), filter.Eq(x => x.TransferenciaId, transferenciaId));
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string companyId, string id) =>
        await _collection.CountDocumentsAsync(x => x.CompanyId == companyId && x.TransferenciaId == id);

    public async Task<DeleteResult> DeleteManyAsync(string companyId, string transferenciaId)
    {
        var builder = Builders<TransferenciaDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.TransferenciaId, transferenciaId));
        return await _collection.DeleteManyAsync(filter);
    }
}
