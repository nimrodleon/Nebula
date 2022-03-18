using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Data.Services;

public interface IComprobanteService
{
    public void SetModel(Venta model);
    public void SetModel(Comprobante model);
    public void SetModel(NotaComprobante model);
    public Task<InvoiceSale> CreateSale(int serie);
    public Task<InvoiceSale> CreateQuickSale(int cajaDiaria);
    public Task<InvoiceSale> CreatePurchase();
    public Task<InvoiceSale> UpdatePurchase();
    public Task<InvoiceNote> CreateNote();
    public Task<InvoiceNote> UpdateNote(int id);
}
