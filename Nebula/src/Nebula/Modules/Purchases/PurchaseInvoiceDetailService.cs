using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Purchases.Dto;
using Nebula.Modules.Purchases.Models;

namespace Nebula.Modules.Purchases;

public interface IPurchaseInvoiceDetailService : ICrudOperationService<PurchaseInvoiceDetail>
{
    Task<List<PurchaseInvoiceDetail>> GetDetailsAsync(string companyId, string purchaseInvoiceId);
    Task<PurchaseInvoiceDetail> CreateAsync(string companyId, string purchaseInvoiceId, ItemCompraDto itemCompra);
    Task<PurchaseInvoiceDetail> UpdateAsync(string companyId, string id, ItemCompraDto itemCompra);
    Task<DeleteResult> DeleteManyAsync(string companyId, string purchaseInvoiceId);
}

public class PurchaseInvoiceDetailService : CrudOperationService<PurchaseInvoiceDetail>, IPurchaseInvoiceDetailService
{
    private readonly ICacheAuthService _cacheAuthService;

    public PurchaseInvoiceDetailService(MongoDatabaseService mongoDatabase, ICacheAuthService cacheAuthService)
        : base(mongoDatabase)
    {
        _cacheAuthService = cacheAuthService;
    }

    public async Task<List<PurchaseInvoiceDetail>> GetDetailsAsync(string companyId, string purchaseInvoiceId)
    {
        return await _collection.Find(x => x.CompanyId == companyId
            && x.PurchaseInvoiceId == purchaseInvoiceId).ToListAsync();
    }

    public async Task<PurchaseInvoiceDetail> CreateAsync(string companyId, string purchaseInvoiceId, ItemCompraDto itemCompra)
    {
        var company = await _cacheAuthService.GetCompanyByIdAsync(companyId);
        var detail = itemCompra.GetDetail(company, purchaseInvoiceId);
        await _collection.InsertOneAsync(detail);
        return detail;
    }

    public async Task<PurchaseInvoiceDetail> UpdateAsync(string companyId, string id, ItemCompraDto itemCompra)
    {
        var company = await _cacheAuthService.GetCompanyByIdAsync(companyId);
        var purchaseInvoiceDetail = await GetByIdAsync(companyId, id);
        itemCompra.Id = purchaseInvoiceDetail.Id;
        purchaseInvoiceDetail = itemCompra.GetDetail(company, purchaseInvoiceDetail.PurchaseInvoiceId);
        await UpdateAsync(purchaseInvoiceDetail.Id, purchaseInvoiceDetail);
        return purchaseInvoiceDetail;
    }

    public async Task<DeleteResult> DeleteManyAsync(string companyId, string purchaseInvoiceId)
    {
        var builder = Builders<PurchaseInvoiceDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.PurchaseInvoiceId, purchaseInvoiceId));
        return await _collection.DeleteManyAsync(filter);
    }

}
