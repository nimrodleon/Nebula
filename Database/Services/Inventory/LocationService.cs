using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory
{
    public class LocationService : CrudOperationService<Location>
    {
        public LocationService(IOptions<DatabaseSettings> options) : base(options) { }

        public async Task<List<Location>> GetByWarehouseIdAsync(string id)
        {
            var builder = Builders<Location>.Filter;
            var filter = builder.Eq(x => x.WarehouseId, id);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
