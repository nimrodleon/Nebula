using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Nebula.Database.Models.Sales;

/// <summary>
/// Detalles de la forma de pago al crédito.
/// </summary>
[BsonIgnoreExtraElements]
public class DetallePagoSale : Generic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// foreignKey in db.
    /// </summary>
    public string InvoiceSale { get; set; } = string.Empty;

    /// <summary>
    /// Monto(s) del pago único o de las cuotas.
    /// </summary>
    public decimal MtoCuotaPago { get; set; } = 0;

    /// <summary>
    /// Fecha(s) de vencimiento del pago único o de las cuotas.
    /// </summary>
    public string FecCuotaPago { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de moneda del pago único o de la cuota.
    /// </summary>
    public string TipMonedaCuotaPago { get; set; } = string.Empty;
}
