using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Inventario.Ajustes;

/// <summary>
/// Items de los productos reinicializados por el ajuste de inventario.
/// </summary>
[BsonIgnoreExtraElements]
public class AjusteInventarioDetail : IGenericDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de Ajuste Inventario.
    /// </summary>
    public string AjusteInventarioId { get; set; } = string.Empty;

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
    /// Cantidad Contada.
    /// </summary>
    public long CantContada { get; set; }
}
