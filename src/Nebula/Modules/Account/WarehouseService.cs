using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Account;

public interface IWarehouseService : ICrudOperationService<Warehouse>
{
    Task<List<Warehouse>> GetAllAsync(string companyId);
    Task<List<string>> GetWarehouseIds(string companyId);
}

public class WarehouseService : CrudOperationService<Warehouse>, IWarehouseService
{
    public WarehouseService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    /// <summary>
    /// Obtiene una lista de todos los almacenes de la base de datos.
    /// </summary>
    /// <returns>Una lista de objetos Warehouse que representa todos los almacenes en la base de datos.</returns>
    public async Task<List<Warehouse>> GetAllAsync(string companyId) =>
        await _collection.Find(x => x.CompanyId == companyId).ToListAsync();

    /// <summary>
    /// Obtiene una lista de los ID de los almacenes registrados en el sistema.
    /// </summary>
    /// <returns>Una lista de strings que contiene los ID de los almacenes.</returns>
    public async Task<List<string>> GetWarehouseIds(string companyId)
    {
        var warehouses = await GetAllAsync(companyId);
        var warehouseArrId = new List<string>();
        warehouses.ForEach(item => warehouseArrId.Add(item.Id));
        return warehouseArrId;
    }
}
