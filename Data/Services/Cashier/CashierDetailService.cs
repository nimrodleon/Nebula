using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Data.Models.Cashier;

namespace Nebula.Data.Services.Cashier;

public class CashierDetailService
{
    private readonly IMongoCollection<CashierDetail> _collection;

    public CashierDetailService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<CashierDetail>("CashierDetails");
    }

    public async Task<List<CashierDetail>> GetListAsync(string id, string? query)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.Eq(x => x.CajaDiaria, id);
        if (!string.IsNullOrEmpty(query))
            filter &= builder.Regex("Contact", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<CashierDetail> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(CashierDetail cashierDetail) =>
        await _collection.InsertOneAsync(cashierDetail);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

    public async Task<long> CountDocumentsAsync(string id) =>
        await _collection.CountDocumentsAsync(x => x.CajaDiaria == id);
}
