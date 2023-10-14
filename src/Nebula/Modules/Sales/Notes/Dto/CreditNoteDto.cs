using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Notes.Dto;

public class CreditNoteDto
{
    public CreditNote CreditNote { get; set; } = new CreditNote();
    public List<CreditNoteDetail> CreditNoteDetails { get; set; } = new List<CreditNoteDetail>();
}
