namespace Nebula.Modules.InvoiceHub.Dto;

public class DetailHub
{
    public string CodProducto { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal MtoValorUnitario { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal MtoBaseIgv { get; set; }
    public int PorcentajeIgv { get; set; }
    public decimal Igv { get; set; }
    public string TipAfeIgv { get; set; } = string.Empty;
    public decimal TotalImpuestos { get; set; }
    public decimal MtoValorVenta { get; set; }
    public decimal MtoPrecioUnitario { get; set; }
}
