using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class InvoiceCancellationResponse
{
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
    public CreditNote CreditNote { get; set; } = new CreditNote();
    public List<CreditNoteDetail> CreditNoteDetail { get; set; } = new List<CreditNoteDetail>();
}
