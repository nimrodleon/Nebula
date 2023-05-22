using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Database;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Ajustes;

public class AjusteInventarioDetailService : CrudOperationService<AjusteInventarioDetail>
{
    private readonly LocationDetailService _locationDetailService;

    public AjusteInventarioDetailService(IOptions<DatabaseSettings> options,
        LocationDetailService locationDetailService) : base(options)
    {
        _locationDetailService = locationDetailService;
    }

    public async Task<List<AjusteInventarioDetail>> GetListAsync(string id)
    {
        var builder = Builders<AjusteInventarioDetail>.Filter;
        var filter = builder.Eq(x => x.AjusteInventarioId, id);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string id) =>
        await _collection.CountDocumentsAsync(x => x.AjusteInventarioId == id);

    public async Task GenerateDetailAsync(string locationId, string ajusteInventarioId)
    {
        var ajusteInventarioDetails = await _locationDetailService.GetAjusteInventarioDetailsAsync(locationId, ajusteInventarioId);
        await _collection.InsertManyAsync(ajusteInventarioDetails);
    }

    public async Task<DeleteResult> DeleteManyAsync(string ajusteInventarioId)
    {
        var builder = Builders<AjusteInventarioDetail>.Filter;
        var filter = builder.Eq(x => x.AjusteInventarioId, ajusteInventarioId);
        return await _collection.DeleteManyAsync(filter);
    }
}
