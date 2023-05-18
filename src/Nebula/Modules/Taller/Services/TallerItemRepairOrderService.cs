using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database;
using Nebula.Database.Services;
using Nebula.Modules.Taller.Models;

namespace Nebula.Modules.Taller.Services;

/// <summary>
/// Servicio Item de la Orden de Reparación.
/// </summary>
public class TallerItemRepairOrderService : CrudOperationService<TallerItemRepairOrder>
{
    public TallerItemRepairOrderService(IOptions<DatabaseSettings> options) : base(options)
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
