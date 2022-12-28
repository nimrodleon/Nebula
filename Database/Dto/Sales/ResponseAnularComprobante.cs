using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

public class ResponseAnularComprobante
{
    public bool JsonFileCreated { get; set; } = false;
    public CreditNote CreditNote { get; set; } = new CreditNote();
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
}
