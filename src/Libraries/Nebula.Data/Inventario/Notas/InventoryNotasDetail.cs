using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Inventario.Notas;

/// <summary>
/// Detalle - Item de la nota de entrada|salida.
/// </summary>
[BsonIgnoreExtraElements]
public class InventoryNotasDetail : IGenericDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador NotaId.
    /// </summary>
    public string InventoryNotasId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Producto.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Producto.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad Requerida.
    /// </summary>
    public int Demanda { get; set; }
}
