using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Inventory;
using Nebula.Database.ViewModels.Common;

namespace Nebula.Database.Services.Inventory;

public class AjusteInventarioService : CrudOperationService<AjusteInventario>
{
    public AjusteInventarioService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<AjusteInventario>> GetListAsync(DateQuery query)
    {
        var builder = Builders<AjusteInventario>.Filter;
        var filter = builder.And(builder.Eq(x => x.Month, query.Month),
            builder.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).SortByDescending(x => x.CreatedAt).ToListAsync();
    }
}
