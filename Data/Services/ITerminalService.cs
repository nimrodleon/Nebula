﻿using System.Threading.Tasks;
using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Data.Services
{
    public interface ITerminalService
    {
        public void SetModel(Sale model);
        public Task<Invoice> SaveInvoice(int cajaDiaria);
    }
}