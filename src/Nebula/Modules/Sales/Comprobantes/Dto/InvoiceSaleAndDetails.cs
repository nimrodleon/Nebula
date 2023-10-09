using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class InvoiceSaleAndDetails
{
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
    public List<InvoiceSaleDetail> InvoiceSaleDetails { get; set; } = new List<InvoiceSaleDetail>();
    public List<DetallePagoSale> DetallePagoSale { get; set; } = new List<DetallePagoSale>();
}
