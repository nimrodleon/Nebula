using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Purchases.Dto;
using Nebula.Modules.Purchases.Models;

namespace Nebula.Modules.Purchases;

public interface IPurchaseInvoiceDetailService : ICrudOperationService<PurchaseInvoiceDetail>
{
    Task<List<PurchaseInvoiceDetail>> GetDetailsAsync(string purchaseInvoiceId);
    Task<PurchaseInvoiceDetail> CreateAsync(string purchaseInvoiceId, ItemCompraDto itemCompra);
    Task<PurchaseInvoiceDetail> UpdateAsync(string id, ItemCompraDto itemCompra);
    Task<DeleteResult> DeleteManyAsync(string purchaseInvoiceId);
}

public class PurchaseInvoiceDetailService : CrudOperationService<PurchaseInvoiceDetail>, IPurchaseInvoiceDetailService
{
    public PurchaseInvoiceDetailService(MongoDatabaseService mongoDatabase)
        : base(mongoDatabase) { }

    public async Task<List<PurchaseInvoiceDetail>> GetDetailsAsync(string purchaseInvoiceId)
    {
        return await _collection.Find(x => x.PurchaseInvoiceId == purchaseInvoiceId).ToListAsync();
    }

    public async Task<PurchaseInvoiceDetail> CreateAsync(string purchaseInvoiceId, ItemCompraDto itemCompra)
    {
        var detail = itemCompra.GetDetail(new Account.Models.Company(), purchaseInvoiceId);
        await _collection.InsertOneAsync(detail);
        return detail;
    }

    public async Task<PurchaseInvoiceDetail> UpdateAsync(string id, ItemCompraDto itemCompra)
    {
        var purchaseInvoiceDetail = await GetByIdAsync(id);
        itemCompra.Id = purchaseInvoiceDetail.Id;
        purchaseInvoiceDetail = itemCompra.GetDetail(new Account.Models.Company(), purchaseInvoiceDetail.PurchaseInvoiceId);
        await UpdateAsync(purchaseInvoiceDetail.Id, purchaseInvoiceDetail);
        return purchaseInvoiceDetail;
    }

    public async Task<DeleteResult> DeleteManyAsync(string purchaseInvoiceId)
    {
        var filter = Builders<PurchaseInvoiceDetail>.Filter.Eq(x =>
                        x.PurchaseInvoiceId, purchaseInvoiceId);
        return await _collection.DeleteManyAsync(filter);
    }

}
