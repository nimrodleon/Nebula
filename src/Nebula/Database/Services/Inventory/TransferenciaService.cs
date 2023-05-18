using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Dto.Common;
using Nebula.Common;

namespace Nebula.Database.Services.Inventory;

public class TransferenciaService : CrudOperationService<Transferencia>
{
    public TransferenciaService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<Transferencia>> GetListAsync(DateQuery query)
    {
        var filter = Builders<Transferencia>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.Month, query.Month),
            filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(dbQuery).SortByDescending(x => x.CreatedAt).ToListAsync();
    }
}
