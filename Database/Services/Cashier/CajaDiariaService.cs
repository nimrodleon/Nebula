using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Cashier;
using Nebula.Database.ViewModels.Common;

namespace Nebula.Database.Services.Cashier;

public class CajaDiariaService : CrudOperationService<CajaDiaria>
{
    public CajaDiariaService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<CajaDiaria>> GetListAsync(DateQuery query)
    {
        var filter = Builders<CajaDiaria>.Filter.And(
            Builders<CajaDiaria>.Filter.Eq(x => x.Month, query.Month),
            Builders<CajaDiaria>.Filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).SortByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task<List<CajaDiaria>> GetCajasAbiertasAsync()
    {
        var filter = Builders<CajaDiaria>.Filter;
        var query = filter.And(filter.Eq(x => x.Status, "ABIERTO"),
            filter.Eq(x => x.Month, DateTime.Now.ToString("MM")),
            filter.Eq(x => x.Year, DateTime.Now.ToString("yyyy")));
        return await _collection.Find(query).ToListAsync();
    }
}
