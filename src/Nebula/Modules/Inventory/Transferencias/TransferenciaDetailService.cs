using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Transferencias;

public interface ITransferenciaDetailService : ICrudOperationService<TransferenciaDetail>
{
    Task<List<TransferenciaDetail>> GetListAsync(string id);
    Task<long> CountDocumentsAsync(string id);
    Task<DeleteResult> DeleteManyAsync(string transferenciaId);
}

public class TransferenciaDetailService : CrudOperationService<TransferenciaDetail>, ITransferenciaDetailService
{
    public TransferenciaDetailService(MongoDatabaseService mongoDatabase) : base(mongoDatabase) { }

    public async Task<List<TransferenciaDetail>> GetListAsync(string id)
    {
        var filter = Builders<TransferenciaDetail>.Filter;
        var query = filter.Eq(x => x.TransferenciaId, id);
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string id) =>
        await _collection.CountDocumentsAsync(x => x.TransferenciaId == id);

    public async Task<DeleteResult> DeleteManyAsync(string transferenciaId)
    {
        var builder = Builders<TransferenciaDetail>.Filter;
        var filter = builder.Eq(x => x.TransferenciaId, transferenciaId);
        return await _collection.DeleteManyAsync(filter);
    }
}
