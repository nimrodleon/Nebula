using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Inventory;
using Nebula.Database.ViewModels.Common;

namespace Nebula.Database.Services.Inventory
{
    public class MaterialService : CrudOperationService<Material>
    {
        public MaterialService(IOptions<DatabaseSettings> options) : base(options) { }

        public async Task<List<Material>> GetListAsync(DateQuery query)
        {
            var filter = Builders<Material>.Filter;
            var dbQuery = filter.And(filter.Eq(x => x.Month, query.Month),
                filter.Eq(x => x.Year, query.Year));
            return await _collection.Find(dbQuery).SortByDescending(x => x.CreatedAt).ToListAsync();
        }
    }
}
