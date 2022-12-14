using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

public class ResponseInvoiceSale
{
    public InvoiceSale? InvoiceSale { get; set; }
    public List<InvoiceSaleDetail>? InvoiceSaleDetails { get; set; }
    public List<TributoSale>? TributoSales { get; set; }
}
