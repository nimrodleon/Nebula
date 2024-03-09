using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Sales.Models;

public class CreditNoteDetail : SaleDetail
{
    /// <summary>
    /// Identificador de la nota de crédito.
    /// </summary>
    public Guid? CreditNoteId { get; set; } = null;

    [ForeignKey(nameof(CreditNoteId))]
    public CreditNote CreditNote { get; set; } = new CreditNote();
}
