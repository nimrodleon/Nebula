namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetNote : LycetBaseSale
{
    public string CodMotivo { get; set; } = string.Empty;
    public string DesMotivo { get; set; } = string.Empty;
    public string TipDocAfectado { get; set; } = string.Empty;
    public string NumDocfectado { get; set; } = string.Empty;
    public LycetSalePerception Perception { get; set; } = new LycetSalePerception();
    public decimal ValorVenta { get; set; }
    public decimal SubTotal { get; set; }
}
