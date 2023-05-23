using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Dto;

public class CreditNoteDto
{
    public CreditNote CreditNote { get; set; } = new CreditNote();
    public List<CreditNoteDetail> CreditNoteDetails { get; set; } = new List<CreditNoteDetail>();
    public List<TributoCreditNote> TributosCreditNote { get; set; } = new List<TributoCreditNote>();
}
