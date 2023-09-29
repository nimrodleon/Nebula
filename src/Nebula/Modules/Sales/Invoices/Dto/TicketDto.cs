using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Invoices.Dto;

public class TicketDto
{
    public Company Company { get; set; } = new Company();
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
    public List<InvoiceSaleDetail> InvoiceSaleDetails { get; set; } = new List<InvoiceSaleDetail>();
    public List<TributoSale> TributoSales { get; set; } = new List<TributoSale>();
}
