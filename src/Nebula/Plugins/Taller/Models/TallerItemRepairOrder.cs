using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Plugins.Taller.Models;

/// <summary>
/// Item de materiales usados en la orden de reparación.
/// </summary>
[BsonIgnoreExtraElements]
public class TallerItemRepairOrder
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
}
