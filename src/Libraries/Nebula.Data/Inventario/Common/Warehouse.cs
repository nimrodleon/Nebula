using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Inventario.Common;

/// <summary>
/// Almacén de Tienda.
/// </summary>
[BsonIgnoreExtraElements]
public class Warehouse : IGenericDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nombre Almacén.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Observación.
    /// </summary>
    public string Remark { get; set; } = string.Empty;
}
