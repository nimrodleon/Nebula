using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Modules.Sales.Models;

[BsonIgnoreExtraElements]
public class InvoiceSale : BaseSale
{
    public string TipoOperacion { get; set; } = "0101";
    public string FecVencimiento { get; set; } = string.Empty;
    public PaymentTerms FormaPago { get; set; } = new PaymentTerms();
    public List<Cuota> Cuotas { get; set; } = new List<Cuota>();
    public string Remark { get; set; } = string.Empty;
    public bool Anulada { get; set; } = false;
}
