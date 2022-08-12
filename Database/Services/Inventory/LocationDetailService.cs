using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory;

public class LocationDetailService : CrudOperationService<LocationDetail>
{
    public LocationDetailService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<LocationDetail>> GetListAsync(string id)
    {
        var filter = Builders<LocationDetail>.Filter;
        var query = filter.Eq(x => x.LocationId, id);
        return await _collection.Find(query).ToListAsync();
    }
}
