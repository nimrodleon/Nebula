using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Database.Models.Common;

/// <summary>
/// Series de facturación.
/// </summary>
public class InvoiceSerie : Generic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador Serie.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Identificar Almacén.
    /// </summary>
    public string Warehouse { get; set; } = string.Empty;

    /// <summary>
    /// Serie Factura.
    /// </summary>
    public string Factura { get; set; } = string.Empty;

    /// <summary>
    /// Contador Factura.
    /// </summary>
    public int CounterFactura { get; set; }

    /// <summary>
    /// Serie Boleta.
    /// </summary>
    public string Boleta { get; set; } = string.Empty;

    /// <summary>
    /// Contador Boleta.
    /// </summary>
    public int CounterBoleta { get; set; }

    /// <summary>
    /// Serie Nota de Venta.
    /// </summary>
    public string NotaDeVenta { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Venta.
    /// </summary>
    public int CounterNotaDeVenta { get; set; }

    /// <summary>
    /// Serie Nota de Crédito.
    /// </summary>
    public string CreditNote { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Crédito.
    /// </summary>
    public int CounterCreditNote { get; set; }

    /// <summary>
    /// Serie Nota de Débito.
    /// </summary>
    public string DebitNote { get; set; } = string.Empty;

    /// <summary>
    /// Contador Nota de Débito.
    /// </summary>
    public int CounterDebitNote { get; set; }
}
