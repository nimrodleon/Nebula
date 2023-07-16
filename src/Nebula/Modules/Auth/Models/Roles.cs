using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;

namespace Nebula.Modules.Auth.Models;

[BsonIgnoreExtraElements]
public class Roles : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;
    public string[] Permisos { get; set; } = new string[] { };
}
