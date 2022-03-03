using System.Collections.Generic;
using Nebula.Data.Models;

namespace Nebula.Data.ViewModels
{
    public class ResponseInvoiceSale
    {
        public InvoiceSale InvoiceSale { get; set; }
        public List<InvoiceSaleDetail> InvoiceSaleDetails { get; set; }
        public List<TributoSale> TributoSales { get; set; }
        public List<InvoiceSaleAccount> InvoiceSaleAccounts { get; set; }
    }
}
