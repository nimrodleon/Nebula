using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Services.Sales;

public class InvoiceSaleDetailService : CrudOperationService<InvoiceSaleDetail>
{
    public InvoiceSaleDetailService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<InvoiceSaleDetail>> GetListAsync(string id) =>
        await _collection.Find(x => x.InvoiceSale == id).ToListAsync();

    public async Task CreateManyAsync(List<InvoiceSaleDetail> invoiceSaleDetails) =>
        await _collection.InsertManyAsync(invoiceSaleDetails);

    public async Task<List<InvoiceSaleDetail>> GetItemsByCajaDiaria(string cajaDiaria) =>
        await _collection.Find(x => x.CajaDiaria == cajaDiaria).ToListAsync();

    public async Task RemoveManyAsync(string id) =>
        await _collection.DeleteManyAsync(x => x.InvoiceSale == id);
}