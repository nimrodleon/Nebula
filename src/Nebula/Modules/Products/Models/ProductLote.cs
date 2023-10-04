using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Common.Models;

namespace Nebula.Modules.Products.Models;

public class ProductLote : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    public string CompanyId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Producto.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Lote de Producción.
    /// </summary>
    public string LotNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de Vencimiento.
    /// </summary>
    public string ExpirationDate { get; set; } = string.Empty;

    /// <summary>
    /// Fecha Recordatorio.
    /// </summary>
    public string ReminderDate { get; set; } = string.Empty;
}
