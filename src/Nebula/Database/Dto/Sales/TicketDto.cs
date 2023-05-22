using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Sales.Models;

namespace Nebula.Database.Dto.Sales;

public class TicketDto
{
    public string DigestValue { get; set; } = string.Empty;
    public string TotalEnLetras { get; set; } = string.Empty;
    public Configuration Configuration { get; set; } = new Configuration();
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
    public List<InvoiceSaleDetail> InvoiceSaleDetails { get; set; } = new List<InvoiceSaleDetail>();
    public List<TributoSale> TributoSales { get; set; } = new List<TributoSale>();
}
