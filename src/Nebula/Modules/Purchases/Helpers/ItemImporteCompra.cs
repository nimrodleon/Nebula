using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Purchases.Dto;
using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Purchases.Helpers;

public class ItemImporteCompra
{
    private readonly Configuration _configuration;

    public ItemImporteCompra(Configuration configuration)
    {
        _configuration = configuration;
    }

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
    /// Calcular Importe Factura de Compra.
    /// </summary>
    /// <param name="itemCompra">Item Compra</param>
    /// <returns>ItemImporteCompra</returns>
    public ItemImporteCompra CalcularImporte(ItemCompraDto itemCompra)
    {
        decimal mtoTotalItem = itemCompra.CtdUnidadItem * itemCompra.MtoPrecioCompraUnitario;
        decimal porcentajeIGV = itemCompra.IgvSunat == TipoIGV.Gravado ? _configuration.PorcentajeIgv / 100 + 1 : 1;
        MtoValorCompraItem = mtoTotalItem / porcentajeIGV;
        MtoTriIcbperItem = itemCompra.TriIcbper ? itemCompra.CtdUnidadItem * _configuration.ValorImpuestoBolsa : 0;
        MtoBaseIgvItem = mtoTotalItem / porcentajeIGV;
        MtoIgvItem = mtoTotalItem - MtoBaseIgvItem;
        SumTotTributosItem = MtoIgvItem + MtoTriIcbperItem; // el sistema soporta solo IGV/ICBPER.
        MtoValorUnitario = MtoValorCompraItem / itemCompra.CtdUnidadItem;
        return this;
    }
}
