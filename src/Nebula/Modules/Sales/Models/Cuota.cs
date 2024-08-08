namespace Nebula.Modules.Sales.Models;

public class Cuota
{
    public string Moneda { get; set; } = string.Empty;
    public decimal Monto { get; set; } = decimal.Zero;
    public string FechaPago { get; set; } = string.Empty;
}
