using Nebula.Data.Models.Sales;

namespace Nebula.Data.ViewModels.Sales;

public class ResponseInvoiceSale
{
    public InvoiceSale? InvoiceSale { get; set; }
    public List<InvoiceSaleDetail>? InvoiceSaleDetails { get; set; }
    public List<TributoSale>? TributoSales { get; set; }
}
