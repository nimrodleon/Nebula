namespace Nebula.Modules.Sales.Models;

public class PaymentTerms
{
    public string Moneda { get; set; } = "PEN";
    public string Tipo { get; set; } = "Contado";
    public decimal Monto { get; set; } = decimal.Zero;
}
