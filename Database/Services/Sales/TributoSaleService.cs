using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Services.Sales;

public class TributoSaleService : CrudOperationService<TributoSale>
{
    public TributoSaleService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<TributoSale>> GetListAsync(string id) =>
        await _collection.Find(x => x.InvoiceSale == id).ToListAsync();

    public async Task CreateManyAsync(List<TributoSale> tributoSales) =>
        await _collection.InsertManyAsync(tributoSales);

    public async Task RemoveManyAsync(string id) =>
        await _collection.DeleteManyAsync(x => x.InvoiceSale == id);
}
