namespace Nebula.Modules.Sales.Dto;

public class SumImpVentaDto
{
    /// <summary>
    /// Total Precio de Venta.
    /// </summary>
    public decimal SumPrecioVenta { get; set; }
    /// <summary>
    /// Total valor de venta.
    /// </summary>
    public decimal SumTotValVenta { get; set; }
    /// <summary>
    /// Sumatoria Tributos.
    /// </summary>
    public decimal SumTotTributos { get; set; }
    /// <summary>
    /// Importe total de la venta, cesión en uso o del servicio prestado.
    /// </summary>
    public decimal SumImpVenta { get; set; }
}
