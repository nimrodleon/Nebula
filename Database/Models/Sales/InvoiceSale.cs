using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Database.Models.Sales;

public class InvoiceSale : Generic
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Tipo documento para control interno.
    /// FACTURA|BOLETA|NOTA DE VENTA, (FACTURA|BOLETA|NOTA).
    /// </summary>
    public string DocType { get; set; } = string.Empty;

    /// <summary>
    /// Serie comprobante.
    /// </summary>
    public string Serie { get; set; } = string.Empty;

    /// <summary>
    /// Número comprobante.
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de operación Catálogo: 51, n4
    /// </summary>
    public string TipOperacion { get; set; } = string.Empty;

    /// <summary>
    /// fecha de emisión. Formato: YYYY-MM-DD, an..10
    /// </summary>
    public string FecEmision { get; set; } = string.Empty;

    /// <summary>
    /// hora emisión. Formato: HH:MM:SS, an..14
    /// </summary>
    public string HorEmision { get; set; } = string.Empty;

    /// <summary>
    /// fecha de vencimiento. Formato: YYYY-MM-DD, an..10
    /// Sin Fecha: Por defecto guión -
    /// </summary>
    public string FecVencimiento { get; set; } = "-";

    /// <summary>
    /// Código del domicilio fiscal o de local anexo del emisor.
    /// </summary>
    public string CodLocalEmisor { get; set; } = string.Empty;

    /// <summary>
    /// Forma de pago. Credito / Contado - a7
    /// </summary>
    public string FormaPago { get; set; } = "Contado";

    /// <summary>
    /// ID de contacto.
    /// Usado para editar el contacto del comprobante.
    /// </summary>
    public string ContactId { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de documento de identidad del adquirente o usuario. Catálogo: 6, an1
    /// </summary>
    public string TipDocUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Número de documento de identidad del adquirente o usuario. an..15
    /// </summary>
    public string NumDocUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos y nombres, denominación o razón social del adquirente o usuario. an..1500
    /// </summary>
    public string RznSocialUsuario { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de moneda en la cual se emite la factura electrónica. Catálogo: 2, an3
    /// </summary>
    public string TipMoneda { get; set; } = string.Empty;

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

    /// <summary>
    /// Estado de Situación Facturador SUNAT.
    /// </summary>
    public string SituacionFacturador { get; set; } = string.Empty;

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
