namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class ComprobantesPendientes
{
    public string ComprobanteId { get; set; } = string.Empty;
    public string TipoDoc { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string Correlativo { get; set; } = string.Empty;
    public string FechaEmision { get; set; } = string.Empty;
    public decimal MtoImpVenta { get; set; } = decimal.Zero;
    public string CdrDescription { get; set; } = string.Empty;
}
