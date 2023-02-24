using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Core.Constants;

namespace Nebula.Data.Common;

/// <summary>
/// Datos del Usuario.
/// </summary>
[BsonIgnoreExtraElements]
public class User : IGenericDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Nombre Identificador del Usuario.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Correo Electrónico.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// contraseña Hash.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Permisos de Autorización.
    /// </summary>
    public string Role { get; set; } = AuthRoles.User;
}
