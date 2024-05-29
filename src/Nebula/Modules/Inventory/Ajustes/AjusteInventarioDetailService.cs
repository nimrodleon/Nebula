using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Ajustes;

public interface IAjusteInventarioDetailService : ICrudOperationService<AjusteInventarioDetail>
{
    Task<List<AjusteInventarioDetail>> GetListAsync(string companyId, string ajusteInventarioId);
    Task<long> CountDocumentsAsync(string companyId, string id);
    Task GenerateDetailAsync(string companyId, string locationId, string ajusteInventarioId);
    Task<DeleteResult> DeleteManyAsync(string companyId, string ajusteInventarioId);
}

public class AjusteInventarioDetailService(
    MongoDatabaseService mongoDatabase,
    ILocationDetailService locationDetailService)
    : CrudOperationService<AjusteInventarioDetail>(mongoDatabase), IAjusteInventarioDetailService
{
    public async Task<List<AjusteInventarioDetail>> GetListAsync(string companyId, string ajusteInventarioId)
    {
        var builder = Builders<AjusteInventarioDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.AjusteInventarioId, ajusteInventarioId));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<long> CountDocumentsAsync(string companyId, string id) =>
        await _collection.CountDocumentsAsync(x => x.CompanyId == companyId && x.AjusteInventarioId == id);

    public async Task GenerateDetailAsync(string companyId, string locationId, string ajusteInventarioId)
    {
        var ajusteInventarioDetails = await locationDetailService.GetAjusteInventarioDetailsAsync(companyId, locationId, ajusteInventarioId);
        await _collection.InsertManyAsync(ajusteInventarioDetails);
    }

    public async Task<DeleteResult> DeleteManyAsync(string companyId, string ajusteInventarioId)
    {
        var builder = Builders<AjusteInventarioDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.AjusteInventarioId, ajusteInventarioId));
        return await _collection.DeleteManyAsync(filter);
    }
}
