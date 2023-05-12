using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Common;

namespace Nebula.Database.Services.Common;

public class ProductLoteService : CrudOperationService<ProductLote>
{
    public ProductLoteService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

    public async Task<List<ProductLote>> GetLotesByExpirationDate(string id, string expirationDate)
    {
        var filter = Builders<ProductLote>.Filter.And(
            Builders<ProductLote>.Filter.Eq(x => x.ProductId, id),
            Builders<ProductLote>.Filter.Lte(x => x.ExpirationDate, expirationDate));
        var result = await _collection.Find(filter).ToListAsync();
        return result;
    }

    public async Task<long> GetLoteCountByProductId(string productId)
    {
        var filter = Builders<ProductLote>.Filter.Eq(x => x.ProductId, productId);
        var count = await _collection.CountDocumentsAsync(filter);
        return count;
    }
}
