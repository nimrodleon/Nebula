using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Data.Services;

public interface ITerminalService
{
    public void SetModel(Venta model);
    public Task<InvoiceSale> SaveInvoice(int cajaDiaria);
}
