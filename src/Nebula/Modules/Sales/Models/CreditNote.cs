namespace Nebula.Modules.Sales.Models;

public class CreditNote : BaseSale
{
    public string InvoiceSaleId { get; set; } = string.Empty;
    public string CodMotivo { get; set; } = string.Empty;
    public string DesMotivo { get; set; } = string.Empty;
    public string TipDocAfectado { get; set; } = string.Empty;
    public string NumDocfectado { get; set; } = string.Empty;
}
