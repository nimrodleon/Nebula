using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Inventario.Location;

/// <summary>
/// Configura las Ubicaciones de los almacenes.
/// </summary>
[BsonIgnoreExtraElements]
public class Location : IGenericDocument
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
