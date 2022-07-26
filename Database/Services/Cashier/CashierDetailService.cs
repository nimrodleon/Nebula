using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Database.Models.Cashier;

namespace Nebula.Database.Services.Cashier;

public class CashierDetailService : CrudOperationService<CashierDetail>
{
    public CashierDetailService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<CashierDetail>> GetListAsync(string id, string? query)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.Eq(x => x.CajaDiaria, id);
        if (!string.IsNullOrEmpty(query))
            filter &= builder.Regex("Contact", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string id) =>
        await _collection.CountDocumentsAsync(x => x.CajaDiaria == id);
}
