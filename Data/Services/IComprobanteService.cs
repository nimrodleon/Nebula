﻿using System.Threading.Tasks;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Data.Services
{
    public interface IComprobanteService
    {
        public void SetModel(Venta model);
        public void SetModel(Comprobante model);
        public Task<Invoice> CreateSale(int serie);
        public Task<Invoice> CreateQuickSale(int cajaDiaria);
        public Task<Invoice> CreatePurchase();
        public Task<Invoice> UpdatePurchase();
    }
}
