using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Common.Dto;
using Nebula.Modules.Account;
using Nebula.Modules.Taller.Models;

namespace Nebula.Modules.Taller.Services;

public interface ITallerRepairOrderService : ICrudOperationService<TallerRepairOrder>
{
    Task<TallerRepairOrder> CreateRepairOrderAsync(string companyId, TallerRepairOrder obj);
    Task<List<TallerRepairOrder>> GetRepairOrders(string companyId, string query = "", int limit = 25);
    Task<List<TallerRepairOrder>> GetRepairOrdersMonthly(string companyId, DateQuery dto, int limit = 25);
    Task<TallerRepairOrderTicket> GetTicket(string companyId, string id);
}

/// <summary>
/// Servicio Orden de Reparación.
/// </summary>
public class TallerRepairOrderService : CrudOperationService<TallerRepairOrder>, ITallerRepairOrderService
{
    private readonly ICompanyService _companyService;
    private readonly IInvoiceSerieService _invoiceSerieService;
    private readonly ITallerItemRepairOrderService _itemRepairOrderService;

    public TallerRepairOrderService(MongoDatabaseService mongoDatabase,
        ICompanyService companyService, ITallerItemRepairOrderService itemRepairOrderService,
        IInvoiceSerieService invoiceSerieService) : base(mongoDatabase)
    {
        _companyService = companyService;
        _itemRepairOrderService = itemRepairOrderService;
        _invoiceSerieService = invoiceSerieService;
    }

    public async Task<TallerRepairOrder> CreateRepairOrderAsync(string companyId, TallerRepairOrder obj)
    {
        obj.Id = string.Empty;
        obj.CompanyId = companyId.Trim();
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(companyId, obj.InvoiceSerieId);
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
    public async Task<List<TallerRepairOrder>> GetRepairOrders(string companyId, string query = "", int limit = 25)
    {
        var filter = Builders<TallerRepairOrder>.Filter.In("Status", new List<string>
        {
            TallerRepairOrderStatus.Pendiente, TallerRepairOrderStatus.EnProceso, TallerRepairOrderStatus.Finalizado
        });

        filter = filter & Builders<TallerRepairOrder>.Filter.Eq(x => x.CompanyId, companyId);

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
    public async Task<List<TallerRepairOrder>> GetRepairOrdersMonthly(string companyId, DateQuery dto, int limit = 25)
    {
        var filterDate = Builders<TallerRepairOrder>.Filter.And(
            Builders<TallerRepairOrder>.Filter.Eq(x => x.CompanyId, companyId),
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
    public async Task<TallerRepairOrderTicket> GetTicket(string companyId, string id)
    {
        var repairOrder = await GetByIdAsync(companyId, id);
        var company = await _companyService.GetByIdAsync(companyId);
        var itemsRepairOrder = await _itemRepairOrderService.GetItemsRepairOrder(companyId, repairOrder.Id);
        return new TallerRepairOrderTicket()
        {
            Company = company,
            RepairOrder = repairOrder,
            ItemsRepairOrder = itemsRepairOrder
        };
    }
}
