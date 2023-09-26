using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Common.Models;

namespace Nebula.Modules.Inventory.Models;

[BsonIgnoreExtraElements]
public class Location : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    public string CompanyId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Almacén.
    /// </summary>
    public string WarehouseId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Almacén.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la Ubicación.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
