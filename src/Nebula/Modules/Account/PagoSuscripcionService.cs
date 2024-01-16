using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Account;

public interface IPagoSuscripcionService : ICrudOperationService<PagoSuscripcion>
{
    Task<long> GetTotalSuscripcionesAsync(string userId, string year, string query = "");
    Task<List<PagoSuscripcion>> GetSuscripcionesAsync(string userId, string year, string query = "", int page = 1, int pageSize = 12);
}

public class PagoSuscripcionService : CrudOperationService<PagoSuscripcion>, IPagoSuscripcionService
{
    public PagoSuscripcionService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<List<PagoSuscripcion>> GetSuscripcionesAsync(
        string userId, string year, string query = "",
        int page = 1, int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;
        var builder = Builders<PagoSuscripcion>.Filter;
        var filter = builder.And(builder.Eq(x => x.UserId, userId), builder.Eq(x => x.Year, year));

        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Regex("CompanyName", new BsonRegularExpression(query, "i"));
        }

        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<PagoSuscripcion>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();
    }

    public async Task<long> GetTotalSuscripcionesAsync(
        string userId, string year, string query = "")
    {
        var builder = Builders<PagoSuscripcion>.Filter;
        var filter = builder.And(builder.Eq(x => x.UserId, userId), builder.Eq(x => x.Year, year));

        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Regex("CompanyName", new BsonRegularExpression(query, "i"));
        }

        return await _collection.Find(filter).CountDocumentsAsync();
    }

}
