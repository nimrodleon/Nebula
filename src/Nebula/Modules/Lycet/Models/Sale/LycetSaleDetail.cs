namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetSaleDetail
{
    /// <summary>
    /// Codigo unidad de Medida.
    /// </summary>
    public string Unidad { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de unidades por ítem.
    /// </summary>
    public decimal Cantidad { get; set; }

    public string CodProducto { get; set; } = string.Empty;

    /// <summary>
    /// Codigo de Producto - SUNAT.
    /// </summary>
    public string CodProdSunat { get; set; } = string.Empty;

    /// <summary>
    /// Código de producto GS1.
    /// </summary>
    public string CodProdGS1 { get; set; } = string.Empty;

    /// <summary>
    /// Descripcion del Producto.
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Monto del valor unitario (PrecioUnitario SIN IGV).
    /// </summary>
    public decimal MtoValorUnitario { get; set; }

    public List<LycetCharge> Cargos { get; set; } = new List<LycetCharge>();
    public List<LycetCharge> Descuentos { get; set; } = new List<LycetCharge>();

    public decimal Descuento { get; set; }
    public decimal MtoBaseIgv { get; set; }
    public decimal PorcentajeIgv { get; set; }
    public decimal Igv { get; set; }
    public string TipAfeIgv { get; set; } = string.Empty;
    public decimal MtoBaseIsc { get; set; }
    public decimal PorcentajeIsc { get; set; }
    public decimal Isc { get; set; }
    public string TipSisIsc { get; set; } = string.Empty;
    public decimal MtoBaseOth { get; set; }
    public decimal PorcentajeOth { get; set; }
    public decimal OtroTributo { get; set; }
    public decimal Icbper { get; set; }
    public decimal FactorIcbper { get; set; } = 0.30M;
    public decimal TotalImpuestos { get; set; }

    /// <summary>
    /// Precio de venta unitario por item.
    /// </summary>
    public decimal MtoPrecioUnitario { get; set; }

    /// <summary>
    /// Valor de venta por ítem. (Total).
    /// </summary>
    public decimal MtoValorVenta { get; set; }

    /// <summary>
    /// Valor referencial unitario por ítem en
    /// operaciones no onerosas (gratuita).
    /// </summary>
    public decimal MtoValorGratuito { get; set; }

    public List<LycetDetailAttribute> Atributos { get; set; } = new List<LycetDetailAttribute>();
}
