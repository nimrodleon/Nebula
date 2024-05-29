using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;
using Nebula.Common.Dto;

namespace Nebula.Modules.Inventory.Ajustes;

public interface IAjusteInventarioService : ICrudOperationService<AjusteInventario>
{
    Task<List<AjusteInventario>> GetListAsync(string companyId, DateQuery query);
}

public class AjusteInventarioService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<AjusteInventario>(mongoDatabase), IAjusteInventarioService
{
    public async Task<List<AjusteInventario>> GetListAsync(string companyId, DateQuery query)
    {
        var builder = Builders<AjusteInventario>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Month, query.Month), builder.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).SortByDescending(x => x.CreatedAt).ToListAsync();
    }
}
