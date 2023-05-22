using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Configurations.Models;

namespace Nebula.Modules.Configurations.Warehouses;

public class WarehouseService : CrudOperationService<Warehouse>
{
    public WarehouseService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

    /// <summary>
    /// Obtiene una lista de todos los almacenes de la base de datos.
    /// </summary>
    /// <returns>Una lista de objetos Warehouse que representa todos los almacenes en la base de datos.</returns>
    public async Task<List<Warehouse>> GetAllAsync() => await _collection.Find(_ => true).ToListAsync();

    /// <summary>
    /// Obtiene una lista de los ID de los almacenes registrados en el sistema.
    /// </summary>
    /// <returns>Una lista de strings que contiene los ID de los almacenes.</returns>
    public async Task<List<string>> GetWarehouseIds()
    {
        var warehouses = await GetAllAsync();
        var warehouseArrId = new List<string>();
        warehouses.ForEach(item => warehouseArrId.Add(item.Id));
        return warehouseArrId;
    }
}
