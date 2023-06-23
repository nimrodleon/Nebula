using Microsoft.Extensions.Options;
using Nebula.Common;
using Nebula.Modules.Purchases.Dto;
using Nebula.Modules.Purchases.Models;

namespace Nebula.Modules.Purchases;

public interface IPurchaseInvoiceService : ICrudOperationService<PurchaseInvoice>
{
    Task<PurchaseInvoice> CreateAsync(CabeceraCompraDto cabecera);
}

public class PurchaseInvoiceService : CrudOperationService<PurchaseInvoice>, IPurchaseInvoiceService
{
    public PurchaseInvoiceService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

    public async Task<PurchaseInvoice> CreateAsync(CabeceraCompraDto cabecera)
    {
        var purchase = cabecera.GetPurchaseInvoice();
        await _collection.InsertOneAsync(purchase);
        return purchase;
    }
}
