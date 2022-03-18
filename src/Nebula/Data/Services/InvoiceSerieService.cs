using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Data.Models;

namespace Nebula.Data.Services;

public class InvoiceSerieService
{
    private readonly IMongoCollection<InvoiceSerie> _collection;

    public InvoiceSerieService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<InvoiceSerie>("InvoiceSeries");
    }

    public async Task<List<InvoiceSerie>> GetListAsync(string? query)
    {
        var filter = Builders<InvoiceSerie>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<InvoiceSerie>.Filter.Regex("Name", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<InvoiceSerie> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(InvoiceSerie invoiceSerie) =>
        await _collection.InsertOneAsync(invoiceSerie);

    public async Task UpdateAsync(string id, InvoiceSerie invoiceSerie) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, invoiceSerie);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
