using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Database.Helpers;

namespace Nebula.Database.Models.Inventory;

public class ProductStock : Generic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador Almacén.
    /// </summary>
    public string WarehouseId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador Producto.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de Registro.
    /// </summary>
    public string Type { get; set; } = InventoryType.ENTRADA;

    /// <summary>
    /// Entrada de Inventario.
    /// </summary>
    public int Entrada { get; set; }

    /// <summary>
    /// Salida de Inventario.
    /// </summary>
    public int Salida { get; set; }
}
