using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Ajustes;

public interface IAjusteInventarioDetailService : ICrudOperationService<AjusteInventarioDetail>
{
    Task<List<AjusteInventarioDetail>> GetListAsync(string id);
    Task<long> CountDocumentsAsync(string id);
    Task GenerateDetailAsync(string locationId, string ajusteInventarioId);
    Task<DeleteResult> DeleteManyAsync(string ajusteInventarioId);
}

public class AjusteInventarioDetailService : CrudOperationService<AjusteInventarioDetail>, IAjusteInventarioDetailService
{
    private readonly ILocationDetailService _locationDetailService;

    public AjusteInventarioDetailService(MongoDatabaseService mongoDatabase,
        ILocationDetailService locationDetailService) : base(mongoDatabase)
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
