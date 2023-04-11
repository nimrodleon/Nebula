using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Database.Models.Common;

/// <summary>
/// Series de facturación.
/// </summary>
[BsonIgnoreExtraElements]
public class InvoiceSerie : IGeneric
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador Serie.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Almacén.
    /// </summary>
    public string WarehouseId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Almacén.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Serie Nota de Venta.
    /// </summary>
    public string NotaDeVenta { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Venta.
    /// </summary>
    public int CounterNotaDeVenta { get; set; }

    /// <summary>
    /// Serie Boleta.
    /// </summary>
    public string Boleta { get; set; } = string.Empty;

    /// <summary>
    /// Contador Boleta.
    /// </summary>
    public int CounterBoleta { get; set; }

    /// <summary>
    /// Serie Factura.
    /// </summary>
    public string Factura { get; set; } = string.Empty;

    /// <summary>
    /// Contador Factura.
    /// </summary>
    public int CounterFactura { get; set; }

    /// <summary>
    /// Serie Nota de Crédito Boleta.
    /// </summary>
    public string CreditNoteBoleta { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Crédito Boleta.
    /// </summary>
    public int CounterCreditNoteBoleta { get; set; }

    /// <summary>
    /// Serie Nota de Crédito Factura.
    /// </summary>
    public string CreditNoteFactura { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Crédito Factura.
    /// </summary>
    public int CounterCreditNoteFactura { get; set; }
}
