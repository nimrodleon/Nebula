using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data.Models.Sales;

namespace Nebula.Data.Services.Sales;

public class TributoSaleService
{
    private readonly IMongoCollection<TributoSale> _collection;

    public TributoSaleService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<TributoSale>("TributoSales");
    }

    public async Task<List<TributoSale>> GetListAsync(string id) =>
        await _collection.Find(x => x.InvoiceSale == id).ToListAsync();

    public async Task CreateAsync(List<TributoSale> tributoSales) =>
        await _collection.InsertManyAsync(tributoSales);
}
