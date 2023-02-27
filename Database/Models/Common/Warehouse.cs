using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Database.Models.Common;

[BsonIgnoreExtraElements]
public class Warehouse : Generic
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
