using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Modules.Sales.Models;

[BsonIgnoreExtraElements]
public class InvoiceSaleDetail : SaleDetail
{
    /// <summary>
    /// Identificador del comprobante.
    /// </summary>
    public string InvoiceSaleId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador CajaDiaria.
    /// </summary>
    public string CajaDiariaId { get; set; } = "-";
}
