using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data.Models.Cashier;
using Nebula.Data.ViewModels.Common;

namespace Nebula.Data.Services.Cashier;

public class CajaDiariaService
{
    private readonly IMongoCollection<CajaDiaria> _collection;

    public CajaDiariaService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<CajaDiaria>("CajasDiaria");
    }

    public async Task<List<CajaDiaria>> GetListAsync(DateQuery query)
    {
        var filter = Builders<CajaDiaria>.Filter.And(
            Builders<CajaDiaria>.Filter.Eq(x => x.Month, query.Month),
            Builders<CajaDiaria>.Filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).SortByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task<CajaDiaria> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(CajaDiaria cajaDiaria) =>
        await _collection.InsertOneAsync(cajaDiaria);

    public async Task UpdateAsync(string id, CajaDiaria cajaDiaria) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, cajaDiaria);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

    public async Task<List<CajaDiaria>> GetCajasAbiertasAsync()
    {
        var filter = Builders<CajaDiaria>.Filter;
        var query = filter.And(filter.Eq(x => x.Status, "ABIERTO"),
            filter.Eq(x => x.Month, DateTime.Now.ToString("MM")),
            filter.Eq(x => x.Year, DateTime.Now.ToString("yyyy")));
        return await _collection.Find(query).ToListAsync();
    }
}
