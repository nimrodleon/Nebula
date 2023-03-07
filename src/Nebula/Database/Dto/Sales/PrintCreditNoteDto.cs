using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

/// <summary>
/// Estructura de datos que se usa para imprimir una nota de crédito.
/// </summary>
public class PrintCreditNoteDto
{
    public string DigestValue { get; set; } = string.Empty;
    public string TotalEnLetras { get; set; } = string.Empty;
    public Configuration Configuration { get; set; } = new Configuration();
    public CreditNote CreditNote { get; set; } = new CreditNote();
    public List<CreditNoteDetail> CreditNoteDetails { get; set; } = new List<CreditNoteDetail>();
    public List<TributoCreditNote> TributosCreditNote { get; set; } = new List<TributoCreditNote>();
}
