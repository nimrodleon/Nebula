using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Dto.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Services.Sales;

public class TributoSaleService : CrudOperationService<TributoSale>
{
    public TributoSaleService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

    public async Task<List<TributoSale>> GetListAsync(string id) =>
        await _collection.Find(x => x.InvoiceSale == id).ToListAsync();

    /// <summary>
    /// Obtener Lista de Tributos Mensual.
    /// </summary>
    /// <param name="date">Datos mes y año</param>
    /// <returns>Lista de tributos</returns>
    public async Task<List<TributoSale>> GetTributosMensual(DateQuery date)
    {
        var builder = Builders<TributoSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.Month, date.Month),
            builder.Eq(x => x.Year, date.Year));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task CreateManyAsync(List<TributoSale> tributoSales) =>
        await _collection.InsertManyAsync(tributoSales);

    public async Task RemoveManyAsync(string id) =>
        await _collection.DeleteManyAsync(x => x.InvoiceSale == id);
}
