using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Models.Sales;

/// <summary>
/// Cuentas por cobrar comprobante de venta.
/// </summary>
public class InvoiceSaleAccount
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// ID Comprobante de Venta.
    /// </summary>
    public string InvoiceSale { get; set; } = string.Empty;

    /// <summary>
    /// Serie comprobante.
    /// </summary>
    public string Serie { get; set; } = string.Empty;

    /// <summary>
    /// Número comprobante.
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Estado Cuenta (PENDIENTE|COBRADO|ANULADO).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Número de Cuota.
    /// </summary>
    public int Cuota { get; set; }

    /// <summary>
    /// Monto Cuenta.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Saldo de la Cuenta.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Fecha Vencimiento.
    /// </summary>
    public string EndDate { get; set; } = string.Empty;

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
