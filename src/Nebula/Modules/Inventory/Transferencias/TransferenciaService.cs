using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;
using Nebula.Common.Dto;

namespace Nebula.Modules.Inventory.Transferencias;

public interface ITransferenciaService : ICrudOperationService<Transferencia>
{
    Task<List<Transferencia>> GetListAsync(string companyId, DateQuery query);
}

public class TransferenciaService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<Transferencia>(mongoDatabase), ITransferenciaService
{
    public async Task<List<Transferencia>> GetListAsync(string companyId, DateQuery query)
    {
        var filter = Builders<Transferencia>.Filter;
        var dbQuery = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.Month, query.Month), filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(dbQuery).SortByDescending(x => x.CreatedAt).ToListAsync();
    }
}
