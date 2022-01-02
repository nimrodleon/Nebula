using System.Threading.Tasks;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Data.Services
{
    public interface IComprobanteService
    {
        public void SetModel(Venta model);
        public void SetModel(Comprobante model);
        public Task<Invoice> SaveSale(int serie);
        public Task<Invoice> SaveQuickSale(int cajaDiaria);
        public Task<Invoice> SavePurchase();
    }
}
