using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Database.Models.Common;

/// <summary>
/// categoría de productos.
/// </summary>
[BsonIgnoreExtraElements]
public class Category : Generic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
