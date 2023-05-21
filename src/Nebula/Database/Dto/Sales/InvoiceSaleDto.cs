using Nebula.Modules.Sales.Models;

namespace Nebula.Database.Dto.Sales;

public class InvoiceSaleDto
{
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
    public List<InvoiceSaleDetail> InvoiceSaleDetails { get; set; } = new List<InvoiceSaleDetail>();
    public List<TributoSale> TributoSales { get; set; } = new List<TributoSale>();
    public List<DetallePagoSale> DetallePagoSales { get; set; } = new List<DetallePagoSale>();
}
