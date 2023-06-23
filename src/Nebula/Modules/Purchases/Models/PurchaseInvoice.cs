using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Common.Models;

namespace Nebula.Modules.Purchases.Models;

[BsonIgnoreExtraElements]
public class PurchaseInvoice : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// fecha de emisión. Formato: YYYY-MM-DD, an..10
    /// </summary>
    public string FecEmision { get; set; } = string.Empty;

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
    /// fecha de vencimiento. Formato: YYYY-MM-DD, an..10
    /// Sin Fecha: Por defecto guión -
    /// </summary>
    public string FecVencimiento { get; set; } = "-";

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
    /// Tipo de documento de identidad del proveedor.
    /// </summary>
    public string TipDocProveedor { get; set; } = string.Empty;

    /// <summary>
    /// Número de documento de identidad del proveedor.
    /// </summary>
    public string NumDocProveedor { get; set; } = string.Empty;

    /// <summary>
    /// Apellidos y nombres, denominación o razón social del proveedor.
    /// </summary>
    public string RznSocialProveedor { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de moneda en la cual se emite la factura electrónica. Catálogo: 2, an3
    /// </summary>
    public string TipMoneda { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de cambio
    /// </summary>
    public decimal TipoCambio { get; set; } = 1;

    /// <summary>
    /// Sumatoria Tributos. an..15|n(12,2)
    /// </summary>
    public decimal SumTotTributos { get; set; }

    /// <summary>
    /// Total valor de compra. an..15|n(12,2)
    /// </summary>
    public decimal SumTotValCompra { get; set; }

    /// <summary>
    /// Total Precio de Compra. an..15|n(12,2)
    /// </summary>
    public decimal SumPrecioCompra { get; set; }

    /// <summary>
    /// Importe total de la compra. an..15|n(12,2)
    /// </summary>
    public decimal SumImpCompra { get; set; }

    /// <summary>
    /// Año de registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
