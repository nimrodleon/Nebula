using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Core.Constants;

namespace Nebula.Data.Ventas.Invoice;

/// <summary>
/// Detalle - Boleta|Factura de venta.
/// </summary>
[BsonIgnoreExtraElements]
public class InvoiceSaleDetail : IGenericDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador CajaDiaria.
    /// </summary>
    public string CajaDiaria { get; set; } = "-";

    /// <summary>
    /// foreignKey cabecera.
    /// </summary>
    public string InvoiceSale { get; set; } = string.Empty;

    /// <summary>
    /// Tipo registro producto o servicio.
    /// </summary>
    public string TipoItem { get; set; } = string.Empty;

    /// <summary>
    /// Código de unidad de medida por ítem. Catálogo: 3, an..3
    /// </summary>
    public string CodUnidadMedida { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de unidades por ítem. an..23|n(12,10)
    /// </summary>
    public decimal CtdUnidadItem { get; set; }

    /// <summary>
    /// Código de producto. an..30
    /// </summary>
    public string CodProducto { get; set; } = string.Empty;

    /// <summary>
    /// Código producto SUNAT. Catálogo: 25, an..8
    /// Sin Código: Por defecto guión -
    /// </summary>
    public string CodProductoSunat { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del servicio prestado, bien vendido o cedido en uso, indicando las características.
    /// </summary>
    public string DesItem { get; set; } = string.Empty;

    /// <summary>
    /// Valor Unitario (cac:InvoiceLine/cac:Price/cbc:PriceAmount). an..23|n(12,10)
    /// </summary>
    public decimal MtoValorUnitario { get; set; }

    /// <summary>
    /// Sumatoria Tributos por item. an..15|n(12,2)
    /// </summary>
    public decimal SumTotTributosItem { get; set; }

    /// <summary>
    /// Tributo: Códigos de tipos de tributos IGV. Catálogo: 5, n4
    /// </summary>
    public string CodTriIgv { get; set; } = string.Empty;

    /// <summary>
    /// Tributo: Monto de IGV por ítem. an..15|n(12,2)
    /// </summary>
    public decimal MtoIgvItem { get; set; }

    /// <summary>
    /// Tributo: Base Imponible IGV por Item. n(12,2)
    /// </summary>
    public decimal MtoBaseIgvItem { get; set; }

    /// <summary>
    /// Tributo: Nombre de tributo por item. an..4 Catálogo: 5,name
    /// </summary>
    public string NomTributoIgvItem { get; set; } = string.Empty;

    /// <summary>
    /// Tributo: Código de tipo de tributo por Item. Catálogo: 5 an4
    /// </summary>
    public string CodTipTributoIgvItem { get; set; } = string.Empty;

    /// <summary>
    /// Tributo: Afectación al IGV por ítem. Catálogo: 7 an2
    /// </summary>
    public string TipAfeIgv { get; set; } = string.Empty;

    /// <summary>
    /// Tributo: Porcentaje de IGV. an..5 =18.0
    /// Colocar 18.00 para expresar 18%
    /// </summary>
    public decimal PorIgvItem { get; set; } = 0;

    /// <summary>
    /// Tributo ICBPER: Códigos de tipos de tributos ICBPER. Catálogo: 5 n4
    /// </summary>
    public string CodTriIcbper { get; set; } = string.Empty;

    /// <summary>
    /// Tributo ICBPER: Monto de tributo ICBPER por iItem. an..15|n(12,2)
    /// </summary>
    public decimal MtoTriIcbperItem { get; set; }

    /// <summary>
    /// Tributo ICBPER: Cantidad de bolsas plásticas por Item. n(5)
    /// </summary>
    public int CtdBolsasTriIcbperItem { get; set; }

    /// <summary>
    /// Tributo ICBPER:  Nombre de tributo ICBPER por item. Catálogo: 5 an..4
    /// </summary>
    public string NomTributoIcbperItem { get; set; } = string.Empty;

    /// <summary>
    /// Tributo ICBPER: Código de tipo de tributo ICBPER por Item. Catálogo: 5 an4
    /// </summary>
    public string CodTipTributoIcbperItem { get; set; } = string.Empty;

    /// <summary>
    /// Tributo ICBPER: Monto de tributo ICBPER por Unidad. an..5
    /// PerUnitAmount - Colocar 0.10 para expresar 0.10 soles / unidad dependiendo del año.
    /// '0.10 año 2019 ; 0.20 año 2020 …
    /// </summary>
    public decimal MtoTriIcbperUnidad { get; set; }

    /// <summary>
    /// Precio de venta unitario cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice an..23|n(12,10)
    /// </summary>
    public decimal MtoPrecioVentaUnitario { get; set; }

    /// <summary>
    /// Valor de venta por Item cac:InvoiceLine/cbc:LineExtensionAmount an..15|n(12,2)
    /// </summary>
    public decimal MtoValorVentaItem { get; set; }

    /// <summary>
    /// Valor REFERENCIAL unitario (gratuitos) cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice
    /// </summary>
    public decimal MtoValorReferencialUnitario { get; set; } = 0;

    /// <summary>
    /// Identificador del Almacén.
    /// </summary>
    public string WarehouseId { get; set; } = string.Empty;

    /// <summary>
    /// Control de Inventario.
    /// </summary>
    public string ControlStock { get; set; } = TipoControlStock.None;
}
