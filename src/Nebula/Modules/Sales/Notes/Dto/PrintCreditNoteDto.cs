using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Notes.Dto;

/// <summary>
/// Estructura de datos que se usa para imprimir una nota de crédito.
/// </summary>
public class PrintCreditNoteDto
{
    public Company Company { get; set; } = new Company();
    public CreditNote CreditNote { get; set; } = new CreditNote();
    public List<CreditNoteDetail> CreditNoteDetails { get; set; } = new List<CreditNoteDetail>();
    public List<TributoCreditNote> TributosCreditNote { get; set; } = new List<TributoCreditNote>();
}
