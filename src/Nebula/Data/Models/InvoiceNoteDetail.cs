using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models;

// TODO: refactoring.
public class InvoiceNoteDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// foreignKey in db.
    /// </summary>
    public int? InvoiceNoteId { get; set; }

    /// <summary>
    /// propiedad de relación.
    /// </summary>
    [JsonIgnore]
    [ForeignKey("InvoiceNoteId")]
    public InvoiceNote InvoiceNote { get; set; }

    /// <summary>
    /// Código de unidad de medida por ítem. Catálogo: 3, an..3
    /// </summary>
    [MaxLength(250)]
    public string CodUnidadMedida { get; set; }

    /// <summary>
    /// Cantidad de unidades por ítem. an..23|n(12,10)
    /// </summary>
    public decimal? CtdUnidadItem { get; set; }

    /// <summary>
    /// Código de producto. an..30
    /// </summary>
    [MaxLength(250)]
    public string CodProducto { get; set; }

    /// <summary>
    /// Código producto SUNAT. Catálogo: 25, an..8
    /// Sin Código: Por defecto guión -
    /// </summary>
    [MaxLength(250)]
    public string CodProductoSunat { get; set; }

    /// <summary>
    /// Descripción detallada del servicio prestado, bien vendido o cedido en uso, indicando las características.
    /// </summary>
    [MaxLength(500)]
    public string DesItem { get; set; }

    /// <summary>
    /// Valor Unitario (cac:InvoiceLine/cac:Price/cbc:PriceAmount). an..23|n(12,10)
    /// </summary>
    public decimal? MtoValorUnitario { get; set; }

    /// <summary>
    /// Sumatoria Tributos por item. an..15|n(12,2)
    /// </summary>
    public decimal? SumTotTributosItem { get; set; }

    /// <summary>
    /// Tributo: Códigos de tipos de tributos IGV. Catálogo: 5, n4
    /// </summary>
    [MaxLength(250)]
    public string CodTriIgv { get; set; }

    /// <summary>
    /// Tributo: Monto de IGV por ítem. an..15|n(12,2)
    /// </summary>
    public decimal? MtoIgvItem { get; set; }

    /// <summary>
    /// Tributo: Base Imponible IGV por Item. n(12,2)
    /// </summary>
    public decimal? MtoBaseIgvItem { get; set; }

    /// <summary>
    /// Tributo: Nombre de tributo por item. an..4 Catálogo: 5,name
    /// </summary>
    [MaxLength(250)]
    public string NomTributoIgvItem { get; set; }

    /// <summary>
    /// Tributo: Código de tipo de tributo por Item. Catálogo: 5 an4
    /// </summary>
    [MaxLength(250)]
    public string CodTipTributoIgvItem { get; set; }

    /// <summary>
    /// Tributo: Afectación al IGV por ítem. Catálogo: 7 an2
    /// </summary>
    [MaxLength(250)]
    public string TipAfeIgv { get; set; }

    /// <summary>
    /// Tributo: Porcentaje de IGV. an..5 =18.0
    /// Colocar 18.00 para expresar 18%
    /// </summary>
    [MaxLength(250)]
    public string PorIgvItem { get; set; }

    /// <summary>
    /// Tributo ICBPER: Códigos de tipos de tributos ICBPER. Catálogo: 5 n4
    /// </summary>
    [MaxLength(250)]
    public string CodTriIcbper { get; set; }

    /// <summary>
    /// Tributo ICBPER: Monto de tributo ICBPER por iItem. an..15|n(12,2)
    /// </summary>
    public decimal? MtoTriIcbperItem { get; set; }

    /// <summary>
    /// Tributo ICBPER: Cantidad de bolsas plásticas por Item. n(5)
    /// </summary>
    public int? CtdBolsasTriIcbperItem { get; set; }

    /// <summary>
    /// Tributo ICBPER:  Nombre de tributo ICBPER por item. Catálogo: 5 an..4
    /// </summary>
    [MaxLength(250)]
    public string NomTributoIcbperItem { get; set; }

    /// <summary>
    /// Tributo ICBPER: Código de tipo de tributo ICBPER por Item. Catálogo: 5 an4
    /// </summary>
    [MaxLength(250)]
    public string CodTipTributoIcbperItem { get; set; }

    /// <summary>
    /// Tributo ICBPER: Monto de tributo ICBPER por Unidad. an..5
    /// PerUnitAmount - Colocar 0.10 para expresar 0.10 soles / unidad dependiendo del año.
    /// '0.10 año 2019 ; 0.20 año 2020 …
    /// </summary>
    public decimal? MtoTriIcbperUnidad { get; set; }

    /// <summary>
    /// Precio de venta unitario cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice an..23|n(12,10)
    /// </summary>
    public decimal? MtoPrecioVentaUnitario { get; set; }

    /// <summary>
    /// Valor de venta por Item cac:InvoiceLine/cbc:LineExtensionAmount an..15|n(12,2)
    /// </summary>
    public decimal? MtoValorVentaItem { get; set; }

    /// <summary>
    /// Valor REFERENCIAL unitario (gratuitos) cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice an..15|n(12,2)
    /// Excluyente con el campo 22. Sin Valor Por defecto guión -
    /// </summary>
    public decimal? MtoValorReferencialUnitario { get; set; }
}
