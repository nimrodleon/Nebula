using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Database.Helpers;

namespace Nebula.Database.Models.Inventory;

[BsonIgnoreExtraElements]
public class ProductStock : IGeneric
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
    /// Cantidad de Productos.
    /// </summary>
    public long Quantity { get; set; }
}
