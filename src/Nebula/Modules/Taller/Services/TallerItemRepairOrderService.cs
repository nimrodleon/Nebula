using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Taller.Models;

namespace Nebula.Modules.Taller.Services;

public interface ITallerItemRepairOrderService : ICrudOperationService<TallerItemRepairOrder>
{
    Task<List<TallerItemRepairOrder>> GetItemsRepairOrder(string companyId, string id);
}

/// <summary>
/// Servicio Item de la Orden de Reparación.
/// </summary>
public class TallerItemRepairOrderService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<TallerItemRepairOrder>(mongoDatabase), ITallerItemRepairOrderService
{
    /// <summary>
    /// Obtener lista de Items de la orden de reparación.
    /// </summary>
    /// <param name="id">Id Orden Reparación</param>
    /// <returns>Lista de Items - Orden de Reparación</returns>
    public async Task<List<TallerItemRepairOrder>> GetItemsRepairOrder(string companyId, string id)
    {
        var builder = Builders<TallerItemRepairOrder>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.RepairOrderId, id));
        return await _collection.Find(filter).ToListAsync();
    }
}
