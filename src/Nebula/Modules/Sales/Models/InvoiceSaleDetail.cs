using Nebula.Modules.Cashier.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Modules.Sales.Models;

public class InvoiceSaleDetail : SaleDetail
{
    /// <summary>
    /// Identificador del comprobante.
    /// </summary>
    public Guid? InvoiceSaleId { get; set; } = null;

    [ForeignKey(nameof(InvoiceSaleId))]
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();

    /// <summary>
    /// Identificador CajaDiaria.
    /// </summary>
    public Guid? CajaDiariaId { get; set; } = null;

    [ForeignKey(nameof(CajaDiariaId))]
    public CajaDiaria CajaDiaria { get; set; } = new CajaDiaria();
}
