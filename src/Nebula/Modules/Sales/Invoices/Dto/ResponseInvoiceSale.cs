using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Invoices.Dto;

public class ResponseInvoiceSale
{
    public InvoiceSale? InvoiceSale { get; set; }
    public List<InvoiceSaleDetail>? InvoiceSaleDetails { get; set; }
    public List<TributoSale>? TributoSales { get; set; }
}
