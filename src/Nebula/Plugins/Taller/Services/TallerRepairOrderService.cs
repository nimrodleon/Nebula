using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Database;
using Nebula.Database.Services;
using Nebula.Plugins.Taller.Models;

namespace Nebula.Plugins.Taller.Services;

/// <summary>
/// Servicio Orden de Reparación.
/// </summary>
public class TallerRepairOrderService : CrudOperationService<TallerRepairOrder>
{
    public TallerRepairOrderService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

    /// <summary>
    /// Obtener la lista de ordenes de servicio pendientes por entregar.
    /// </summary>
    /// <param name="query">Nombre del Cliente</param>
    /// <param name="limit">Cantidad de registros</param>
    /// <returns>Lista de Ordenes de Servicio</returns>
    public async Task<List<TallerRepairOrder>> GetRepairOrders(string? query = "", int limit = 25)
    {
        var filter = Builders<TallerRepairOrder>.Filter.In("Status", new List<string>
        {
            TallerRepairOrderStatus.Pendiente, TallerRepairOrderStatus.EnProceso, TallerRepairOrderStatus.Finalizado
        });
        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter &
                     Builders<TallerRepairOrder>.Filter.Regex("NombreCliente",
                         new BsonRegularExpression(query.ToUpper(), "i"));
        }

        var result = await _collection.Find(filter)
            .Sort(new SortDefinitionBuilder<TallerRepairOrder>().Descending("$natural"))
            .Limit(limit).ToListAsync();
        return result;
    }
}
