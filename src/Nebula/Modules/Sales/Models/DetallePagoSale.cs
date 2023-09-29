using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;

namespace Nebula.Modules.Sales.Models;

/// <summary>
/// Detalles de la forma de pago al crédito.
/// </summary>
[BsonIgnoreExtraElements]
public class DetallePagoSale : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// foreignKey in db.
    /// </summary>
    public string InvoiceSaleId { get; set; } = string.Empty;

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
