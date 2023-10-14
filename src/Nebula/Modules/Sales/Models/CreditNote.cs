using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Modules.Sales.Models;

[BsonIgnoreExtraElements]
public class CreditNote : BaseSale
{
    public string CodMotivo { get; set; } = string.Empty;
    public string DesMotivo { get; set; } = string.Empty;
    public string TipDocAfectado { get; set; } = string.Empty;
    public string NumDocfectado { get; set; } = string.Empty;
}
