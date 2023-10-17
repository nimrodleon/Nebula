using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Inventory.Dto;

public class InvoiceSaleStockDto
{
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
    public List<InvoiceSaleDetail> InvoiceSaleDetails { get; set; } = new List<InvoiceSaleDetail>();
}
