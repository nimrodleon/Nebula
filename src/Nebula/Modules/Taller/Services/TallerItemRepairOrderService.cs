using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Taller.Models;

namespace Nebula.Modules.Taller.Services;

public interface ITallerItemRepairOrderService : ICrudOperationService<TallerItemRepairOrder>
{
    Task<List<TallerItemRepairOrder>> GetItemsRepairOrder(string id);
}

/// <summary>
/// Servicio Item de la Orden de Reparación.
/// </summary>
public class TallerItemRepairOrderService : CrudOperationService<TallerItemRepairOrder>, ITallerItemRepairOrderService
{
    public TallerItemRepairOrderService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    /// <summary>
    /// Obtener lista de Items de la orden de reparación.
    /// </summary>
    /// <param name="id">Id Orden Reparación</param>
    /// <returns>Lista de Items - Orden de Reparación</returns>
    public async Task<List<TallerItemRepairOrder>> GetItemsRepairOrder(string id)
    {
        var filter = Builders<TallerItemRepairOrder>.Filter.Eq(x => x.RepairOrderId, id);
        return await _collection.Find(filter).ToListAsync();
    }
}
