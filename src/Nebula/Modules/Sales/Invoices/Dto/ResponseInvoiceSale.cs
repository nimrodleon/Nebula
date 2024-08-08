using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Invoices.Dto;

public class ResponseInvoiceSale
{
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
    public List<InvoiceSaleDetail> InvoiceSaleDetails { get; set; } = new List<InvoiceSaleDetail>();
}
