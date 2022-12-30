using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Nebula.Database.Models.Inventory;

[BsonIgnoreExtraElements]
public class InventoryNotasDetail : Generic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Clave foranea NotaId.
    /// </summary>
    public string InventoryNotasId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Producto.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Producto.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad Requerida.
    /// </summary>
    public int Demanda { get; set; }
}
