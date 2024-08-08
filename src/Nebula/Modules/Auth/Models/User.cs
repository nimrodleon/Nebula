using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Common.Models;
using Nebula.Modules.Auth.Helpers;
using System.Text.Json.Serialization;

namespace Nebula.Modules.Auth.Models;

[BsonIgnoreExtraElements]
public class User : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
    public string UserType { get; set; } = UserTypeSystem.Customer;
    public bool IsEmailVerified { get; set; } = false;
    [JsonIgnore]
    public string EmailValidationToken { get; set; } = string.Empty;
    [JsonIgnore]
    public string ResetPasswordToken { get; set; } = string.Empty;
    public bool Disabled { get; set; } = false;
}
