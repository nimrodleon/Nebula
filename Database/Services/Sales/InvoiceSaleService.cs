using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Services.Sales;

public class InvoiceSaleService : CrudOperationService<InvoiceSale>
{
    public InvoiceSaleService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<InvoiceSale> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<InvoiceSale>> GetByContactIdAsync(string id, string month, string year)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.ContactId, id),
            builder.Eq(x => x.Month, month), builder.Eq(x => x.Year, year));
        return await _collection.Find(filter).ToListAsync();
    }
}
