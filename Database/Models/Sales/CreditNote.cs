using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Nebula.Database.Models.Sales;

public class CreditNote : Generic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de operación Catálogo: 51, n2
    /// </summary>
    public string TipOperacion { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de emisión Formato: YYYY-MM-DD, an..10
    /// </summary>
    public string FecEmision { get; set; } = string.Empty;

    /// <summary>
    /// Hora de Emisión Formato: HH:MM:SS, an..14
    /// </summary>
    public string HorEmision { get; set; } = string.Empty;

    /// <summary>
    /// Código del domicilio fiscal o de local anexo del emisor
    /// </summary>
    public string CodLocalEmisor { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento de identidad del adquirente o usuario. Catálogo 6, an1
    /// </summary>
    public string TipDocUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Número de documento de identidad del adquirente o usuario. an..15
    /// </summary>
    public string NumDocUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos y nombres, denominación o razón social del adquirente o usuario
    /// </summary>
    public string RznSocialUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de moneda en la cual se emite la factura electrónica. Catálogo 2, an3
    /// </summary>
    public string TipMoneda { get; set; } = string.Empty;

    /// <summary>
    /// Código del tipo de Nota  electrónica. Catálogo 10, an2
    /// </summary>
    public string CodMotivo { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de motivo o sustento. Formato: an..250
    /// </summary>
    public string DesMotivo { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento del documento que modifica. Formato: 01 o 03 o 12, an2
    /// </summary>
    public string TipDocAfectado { get; set; } = string.Empty;

    /// <summary>
    /// Serie y número del documento que modifica. Formato: XXXX-99999999, n..13
    /// </summary>
    public string NumDocAfectado { get; set; } = string.Empty;

    /// <summary>
    /// Sumatoria Tributos. an..15|n(12,2)
    /// </summary>
    public decimal SumTotTributos { get; set; }

    /// <summary>
    /// Total valor de venta. an..15|n(12,2)
    /// </summary>
    public decimal SumTotValVenta { get; set; }

    /// <summary>
    /// Total Precio de Venta. an..15|n(12,2)
    /// </summary>
    public decimal SumPrecioVenta { get; set; }

    /// <summary>
    /// Importe total de la venta, cesión en uso o del servicio prestado. an..15|n(12,2)
    /// </summary>
    public decimal SumImpVenta { get; set; }
}
