using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Locations;

public class LocationDetailService : CrudOperationService<LocationDetail>
{
    public LocationDetailService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<LocationDetail>> GetListAsync(string id)
    {
        var builder = Builders<LocationDetail>.Filter;
        var filter = builder.Eq(x => x.LocationId, id);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string id) =>
        await _collection.CountDocumentsAsync(x => x.LocationId == id);

    public async Task<List<AjusteInventarioDetail>> GetAjusteInventarioDetailsAsync(string locationId, string ajusteInventarioId)
    {
        var locationDetails = await GetListAsync(locationId);
        var ajusteInventarioDetails = new List<AjusteInventarioDetail>();
        locationDetails.ForEach(item =>
        {
            ajusteInventarioDetails.Add(new AjusteInventarioDetail()
            {
                Id = string.Empty,
                AjusteInventarioId = ajusteInventarioId,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                CantExistente = -1,
                CantContada = 0,
            });
        });
        return ajusteInventarioDetails;
    }
}
