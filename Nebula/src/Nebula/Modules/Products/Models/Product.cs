using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Common.Models;
using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Products.Models;

[BsonIgnoreExtraElements]
public class Product : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la empresa al que pertenece.
    /// </summary>
    public string CompanyId { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del producto.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Categoría de producto.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Código de Barra.
    /// </summary>
    public string Barcode { get; set; } = "-";

    /// <summary>
    /// Tipo IGV Sunat.
    /// </summary>
    public string IgvSunat { get; set; } = TipoIGV.Gravado;

    /// <summary>
    /// Precio Venta Unitario (Incluye IGV).
    /// </summary>
    public decimal PrecioVentaUnitario { get; set; }

    /// <summary>
    /// Tipo de Bien/Servicio.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Unidad de Medida.
    /// </summary>
    public string UndMedida { get; set; } = string.Empty;

    /// <summary>
    /// Lote de Producción.
    /// </summary>
    public string Lote { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de Vencimiento del producto.
    /// </summary>
    public string FecVencimiento { get; set; } = string.Empty;
}
