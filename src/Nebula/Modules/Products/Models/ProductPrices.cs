using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;

namespace Nebula.Modules.Products.Models;

/// <summary>
/// Precios del producto.
/// </summary>
[BsonIgnoreExtraElements]
public class ProductPrices : IGenericModel
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

    public bool CantMinima { get; set; } = false;
    public string Nombre { get; set; } = string.Empty;
    public string UndMedida { get; set; } = string.Empty;
    public decimal PrecioVenta { get; set; } = decimal.Zero;
    public decimal Contiene { get; set; } = decimal.One;
}
