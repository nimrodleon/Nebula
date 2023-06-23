using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Purchases.Models;

namespace Nebula.Modules.Purchases;

public interface IPurchaseInvoiceDetailService : ICrudOperationService<PurchaseInvoiceDetail>
{
    Task<List<PurchaseInvoiceDetail>> GetDetailsAsync(string purchaseInvoiceId);
}

public class PurchaseInvoiceDetailService : CrudOperationService<PurchaseInvoiceDetail>, IPurchaseInvoiceDetailService
{
    public PurchaseInvoiceDetailService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

    public async Task<List<PurchaseInvoiceDetail>> GetDetailsAsync(string purchaseInvoiceId)
    {
        return await _collection.Find(x => x.PurchaseInvoiceId == purchaseInvoiceId).ToListAsync();
    }

}
