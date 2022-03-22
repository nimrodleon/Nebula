using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Models.Common;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del producto.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Código de Barra.
    /// </summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// Precio de producto.
    /// </summary>
    public decimal Price1 { get; set; }

    /// <summary>
    /// Precio mayor del producto.
    /// </summary>
    public decimal Price2 { get; set; }

    /// <summary>
    /// Cantidad minima para aplicar precio a mayor.
    /// </summary>
    public decimal FromQty { get; set; }

    /// <summary>
    /// Tipo IGV Sunat.
    /// </summary>
    public string IgvSunat { get; set; } = string.Empty;

    /// <summary>
    /// Impuesto a la bolsa plástica.
    /// </summary>
    public string Icbper { get; set; } = string.Empty;

    /// <summary>
    /// Categoría de producto.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Unidad de Medida.
    /// </summary>
    public string UndMedida { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de Bien/Servicio.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Path de la imagen del producto.
    /// </summary>
    public string PathImage { get; set; } = string.Empty;
}
