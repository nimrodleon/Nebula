using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Modules.Auth.Helpers;
using Nebula.Common.Models;
using System.Text.Json.Serialization;

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
    public bool IsEmailVerified { get; set; } = false;
    [JsonIgnore]
    public string EmailValidationToken { get; set; } = string.Empty;
}
