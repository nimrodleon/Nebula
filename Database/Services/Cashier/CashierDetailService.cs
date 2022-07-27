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

    public async Task<List<CashierDetail>> GetEntradaSalidaAsync(string contactId, string month, string year)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Month, month), builder.Eq(x => x.Year, year),
            builder.In("TypeOperation", new List<string>() { "ENTRADA_DE_DINERO", "SALIDA_DE_DINERO" }));
        return await _collection.Find(filter).ToListAsync();
    }
}
