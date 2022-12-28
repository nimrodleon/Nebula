using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory;

public class TransferenciaDetailService : CrudOperationService<TransferenciaDetail>
{
    public TransferenciaDetailService(IOptions<DatabaseSettings> options) : base(options) { }

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
