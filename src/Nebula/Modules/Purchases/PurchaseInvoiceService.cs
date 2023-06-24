using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Common.Dto;
using Nebula.Modules.Purchases.Dto;
using Nebula.Modules.Purchases.Models;

namespace Nebula.Modules.Purchases;

public interface IPurchaseInvoiceService : ICrudOperationService<PurchaseInvoice>
{
    Task<List<PurchaseInvoice>> GetAsync(DateQuery query);
    Task<PurchaseInvoice> CreateAsync(CabeceraCompraDto cabecera);
    Task<PurchaseInvoice> UpdateAsync(string id, CabeceraCompraDto cabecera);
}

public class PurchaseInvoiceService : CrudOperationService<PurchaseInvoice>, IPurchaseInvoiceService
{
    public PurchaseInvoiceService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

    public async Task<List<PurchaseInvoice>> GetAsync(DateQuery query)
    {
        var builder = Builders<PurchaseInvoice>.Filter;
        var filter = builder.And(
            builder.Eq(x => x.Month, query.Month),
            builder.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).SortBy(x => x.FecEmision).ToListAsync();
    }

    public async Task<PurchaseInvoice> CreateAsync(CabeceraCompraDto cabecera)
    {
        var purchase = cabecera.GetPurchaseInvoice();
        await _collection.InsertOneAsync(purchase);
        return purchase;
    }

    public async Task<PurchaseInvoice> UpdateAsync(string id, CabeceraCompraDto cabecera)
    {
        var purchase = await GetByIdAsync(id);
        purchase = cabecera.GetPurchaseInvoice(purchase);
        await UpdateAsync(purchase.Id, purchase);
        return purchase;
    }

}
