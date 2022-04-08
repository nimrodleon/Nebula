using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data.Models.Sales;

namespace Nebula.Data.Services.Sales;

public class InvoiceSaleDetailService
{
    private readonly IMongoCollection<InvoiceSaleDetail> _collection;

    public InvoiceSaleDetailService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<InvoiceSaleDetail>("InvoiceSaleDetails");
    }

    public async Task<List<InvoiceSaleDetail>> GetListAsync(string id) =>
        await _collection.Find(x => x.InvoiceSale == id).ToListAsync();

    public async Task CreateAsync(List<InvoiceSaleDetail> invoiceSaleDetails) =>
        await _collection.InsertManyAsync(invoiceSaleDetails);
}
