using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data.Models.Sales;

namespace Nebula.Data.Services.Sales;

public class InvoiceSaleAccountService
{
    private readonly IMongoCollection<InvoiceSaleAccount> _collection;

    public InvoiceSaleAccountService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<InvoiceSaleAccount>("InvoiceSaleAccounts");
    }

    public async Task<InvoiceSaleAccount> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<InvoiceSaleAccount>> GetListAsync(string id) =>
        await _collection.Find(x => x.InvoiceSale == id).ToListAsync();

    public async Task CreateAsync(InvoiceSaleAccount invoiceSaleAccount) =>
        await _collection.InsertOneAsync(invoiceSaleAccount);
}
