using Nebula.Modules.Purchases.Models;

namespace Nebula.Modules.Purchases.Dto;

public class PurchaseDto
{
    public PurchaseInvoice PurchaseInvoice { get; set; } = new PurchaseInvoice();
    public List<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; } = new List<PurchaseInvoiceDetail>();
}
