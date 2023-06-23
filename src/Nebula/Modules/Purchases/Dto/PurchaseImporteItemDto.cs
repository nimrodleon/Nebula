using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Purchases.Dto;

public class PurchaseImporteItemDto
{
    /// <summary>
    /// Valor Unitario sin Ningún Tributo.
    /// </summary>
    public decimal MtoValorUnitario { get; set; }

    /// <summary>
    /// Sumatoria Tributos por item, = 9 + 16 + 23
    /// </summary>
    public decimal SumTotTributosItem { get; set; }

    /// <summary>
    /// Monto de IGV por ítem.
    /// </summary>
    public decimal MtoIgvItem { get; set; }

    /// <summary>
    /// Base Imponible IGV por Item.
    /// Siempre mayor a cero. Si el item tiene ISC, agregar dicho monto a la base imponible.
    /// </summary>
    public decimal MtoBaseIgvItem { get; set; }

    /// <summary>
    /// Monto de tributo ICBPER por iItem.
    /// </summary>
    public decimal MtoTriIcbperItem { get; set; }

    /// <summary>
    /// Total compra Item sin Ningún Tributo.
    /// </summary>
    public decimal MtoValorCompraItem { get; set; }

    /// <summary>
    /// El mismo valor que ItemComprobante.
    /// Solo para Generar los Tributos del Comprobante.
    /// GRAVADO, EXONERADO, INAFECTO.
    /// </summary>
    public string IgvSunat { get; set; } = TipoIGV.Gravado;
}
