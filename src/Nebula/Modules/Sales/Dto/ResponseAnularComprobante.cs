using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Dto;

public class ResponseAnularComprobante
{
    public bool JsonFileCreated { get; set; } = false;
    public CreditNote CreditNote { get; set; } = new CreditNote();
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
}
