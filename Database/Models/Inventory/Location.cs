using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Database.Models.Inventory;

[BsonIgnoreExtraElements]
public class Location : Generic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Almacén.
    /// </summary>
    public string WarehouseId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Almacén.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la Ubicación.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
