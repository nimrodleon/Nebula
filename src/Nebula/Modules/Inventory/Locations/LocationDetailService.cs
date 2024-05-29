using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Locations;

public interface ILocationDetailService : ICrudOperationService<LocationDetail>
{
    Task<List<LocationDetail>> GetListAsync(string companyId, string id);
    Task<long> CountDocumentsAsync(string companyId, string id);
    Task<List<AjusteInventarioDetail>> GetAjusteInventarioDetailsAsync(string companyId, string locationId, string ajusteInventarioId);
}

public class LocationDetailService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<LocationDetail>(mongoDatabase), ILocationDetailService
{
    public async Task<List<LocationDetail>> GetListAsync(string companyId, string id)
    {
        var builder = Builders<LocationDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.LocationId, id));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync( string companyId, string id) =>
        await _collection.CountDocumentsAsync(x => x.CompanyId == companyId && x.LocationId == id);

    public async Task<List<AjusteInventarioDetail>> GetAjusteInventarioDetailsAsync(string companyId, string locationId, string ajusteInventarioId)
    {
        var locationDetails = await GetListAsync(companyId, locationId);
        var ajusteInventarioDetails = new List<AjusteInventarioDetail>();
        locationDetails.ForEach(item =>
        {
            ajusteInventarioDetails.Add(new AjusteInventarioDetail()
            {
                Id = string.Empty,
                CompanyId = companyId.Trim(),
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
