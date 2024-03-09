using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Sales.Models;

public class Cuota
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Moneda { get; set; } = string.Empty;
    public decimal Monto { get; set; } = decimal.Zero;
    public string FechaPago { get; set; } = string.Empty;
    public Guid? InvoiceSaleId { get; set; } = null;
    [ForeignKey(nameof(InvoiceSaleId))]
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
}
