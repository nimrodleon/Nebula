using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;

namespace Nebula.Modules.Inventory.Models;

[BsonIgnoreExtraElements]
public class TransferenciaDetail : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Clave foranea TransferenciaId.
    /// </summary>
    public string TransferenciaId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Producto.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Producto.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad Existente.
    /// </summary>
    public long CantExistente { get; set; }

    /// <summary>
    /// Cantidad Transferido.
    /// </summary>
    public long CantTransferido { get; set; }

    /// <summary>
    /// Cantidad Restante.
    /// </summary>
    public long CantRestante { get; set; }
}