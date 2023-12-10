using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Modules.Sales.Models;

[BsonIgnoreExtraElements]
public class CreditNoteDetail : SaleDetail
{
    /// <summary>
    /// Identificador de la nota de crédito.
    /// </summary>
    public string CreditNoteId { get; set; } = string.Empty;
}
