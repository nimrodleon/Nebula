using Microsoft.EntityFrameworkCore;

namespace Nebula.Modules.Sales.Models;

[Owned]
public class PaymentTerms
{
    public string Moneda { get; set; } = "PEN";
    public string Tipo { get; set; } = "Contado";
    public decimal Monto { get; set; } = decimal.Zero;
}
