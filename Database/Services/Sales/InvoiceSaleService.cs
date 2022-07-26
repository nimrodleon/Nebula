using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Services.Sales;

public class InvoiceSaleService : CrudOperationService<InvoiceSale>
{
    public InvoiceSaleService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<InvoiceSale> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
}
