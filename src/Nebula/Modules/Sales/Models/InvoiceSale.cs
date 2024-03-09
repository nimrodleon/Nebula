namespace Nebula.Modules.Sales.Models;

public class InvoiceSale : BaseSale
{
    public string TipoOperacion { get; set; } = "0101";
    public string FecVencimiento { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public PaymentTerms FormaPago { get; set; } = new PaymentTerms();
    public ICollection<Cuota> Cuotas { get; set; } = new List<Cuota>();
    public string Remark { get; set; } = string.Empty;
    public bool Anulada { get; set; } = false;
}
