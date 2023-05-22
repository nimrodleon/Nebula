using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Database.Dto.Common;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Taller.Models;

namespace Nebula.Modules.Taller.Services;

/// <summary>
/// Servicio Orden de Reparación.
/// </summary>
public class TallerRepairOrderService : CrudOperationService<TallerRepairOrder>
{
    private readonly CrudOperationService<InvoiceSerie> _invoiceSerieService;
    private readonly TallerItemRepairOrderService _itemRepairOrderService;

    public TallerRepairOrderService(IOptions<DatabaseSettings> options,
        TallerItemRepairOrderService itemRepairOrderService,
        CrudOperationService<InvoiceSerie> invoiceSerieService) : base(options)
    {
        _itemRepairOrderService = itemRepairOrderService;
        _invoiceSerieService = invoiceSerieService;
    }

    public async Task<TallerRepairOrder> CreateRepairOrderAsync(TallerRepairOrder obj)
    {
        obj.Id = string.Empty;
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(obj.InvoiceSerieId);
        obj.Serie = invoiceSerie.NotaDeVenta;
        int numComprobante = invoiceSerie.CounterNotaDeVenta + 1;
        obj.Number = numComprobante.ToString();
        // actualizar serie de facturación.
        invoiceSerie.CounterNotaDeVenta = numComprobante;
        await _invoiceSerieService.UpdateAsync(invoiceSerie.Id, invoiceSerie);
        await _collection.InsertOneAsync(obj);
        return obj;
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

    /// <summary>
    /// Obtener lista de ordenes de servicio archivados y entregados.
    /// </summary>
    /// <param name="dto">Mes, Año y texto de consulta</param>
    /// <param name="limit">Cantidad de Registros</param>
    /// <returns>Lista de Ordenes de Servicio</returns>
    public async Task<List<TallerRepairOrder>> GetRepairOrdersMonthly(DateQuery dto, int limit = 25)
    {
        var filterDate = Builders<TallerRepairOrder>.Filter.And(
            Builders<TallerRepairOrder>.Filter.Eq(x => x.Month, dto.Month),
            Builders<TallerRepairOrder>.Filter.Eq(x => x.Year, dto.Year));
        var filterStatus = Builders<TallerRepairOrder>.Filter.In("Status", new List<string>
        {
            TallerRepairOrderStatus.Entregado, TallerRepairOrderStatus.Archivado
        });
        var filter = filterDate & filterStatus;
        if (!string.IsNullOrWhiteSpace(dto.Query))
        {
            var filterQuery = Builders<TallerRepairOrder>.Filter.Regex("NombreCliente",
                new BsonRegularExpression(dto.Query.ToUpper(), "i"));
            filter = filter & filterQuery;
        }

        var result = await _collection.Find(filter)
            .Sort(new SortDefinitionBuilder<TallerRepairOrder>().Descending("$natural"))
            .Limit(limit).ToListAsync();
        return result;
    }

    /// <summary>
    /// Obtener datos para imprimir.
    /// </summary>
    /// <param name="id">Identificador de la Orden de servicio</param>
    /// <returns>TallerRepairOrderTicket</returns>
    public async Task<TallerRepairOrderTicket> GetTicket(string id)
    {
        var repairOrder = await GetByIdAsync(id);
        var itemsRepairOrder = await _itemRepairOrderService.GetItemsRepairOrder(repairOrder.Id);
        return new TallerRepairOrderTicket()
        {
            RepairOrder = repairOrder,
            ItemsRepairOrder = itemsRepairOrder
        };
    }
}
