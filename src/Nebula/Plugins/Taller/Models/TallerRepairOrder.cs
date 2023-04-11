using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Database.Models;

namespace Nebula.Plugins.Taller.Models;

/// <summary>
/// Orden de Reparación.
/// </summary>
[BsonIgnoreExtraElements]
public class TallerRepairOrder : IGeneric
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
}
