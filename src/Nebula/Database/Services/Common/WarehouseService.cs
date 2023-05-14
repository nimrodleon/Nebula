using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Common;

namespace Nebula.Database.Services.Common;

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
}
