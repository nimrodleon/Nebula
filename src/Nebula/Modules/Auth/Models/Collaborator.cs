using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Models;

namespace Nebula.Modules.Auth.Models;

[BsonIgnoreExtraElements]
public class Collaborator : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string CompanyId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserRole { get; set; } = CompanyRoles.User;
}
