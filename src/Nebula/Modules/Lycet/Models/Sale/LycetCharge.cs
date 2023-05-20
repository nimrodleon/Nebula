namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetCharge
{
    public string CodTipo { get; set; } = string.Empty;
    public decimal Factor { get; set; }
    public decimal Monto { get; set; }
    public decimal MontoBase { get; set; }
}
