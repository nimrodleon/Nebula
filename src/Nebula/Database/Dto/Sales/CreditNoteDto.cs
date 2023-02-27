using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

public class CreditNoteDto
{
    public CreditNote CreditNote { get; set; } = new CreditNote();
    public List<CreditNoteDetail> CreditNoteDetails { get; set; } = new List<CreditNoteDetail>();
    public List<TributoCreditNote> TributosCreditNote { get; set; } = new List<TributoCreditNote>();
}
