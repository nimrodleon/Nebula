using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Invoices;

public interface IInvoiceSaleDetailService : ICrudOperationService<InvoiceSaleDetail>
{
    Task<List<InvoiceSaleDetail>> GetListAsync(string companyId, string invoiceSaleId);
    Task CreateManyAsync(List<InvoiceSaleDetail> invoiceSaleDetails);
    Task<List<InvoiceSaleDetail>> GetItemsByCajaDiaria(string companyId, string cajaDiariaId);
    Task RemoveManyAsync(string companyId, string invoiceSaleId);
}

public class InvoiceSaleDetailService : CrudOperationService<InvoiceSaleDetail>, IInvoiceSaleDetailService
{
    public InvoiceSaleDetailService(MongoDatabaseService mongoDatabase) : base(mongoDatabase) { }

    public async Task<List<InvoiceSaleDetail>> GetListAsync(string companyId, string invoiceSaleId) =>
        await _collection.Find(x => x.CompanyId == companyId && x.InvoiceSaleId == invoiceSaleId).ToListAsync();

    public async Task CreateManyAsync(List<InvoiceSaleDetail> invoiceSaleDetails) =>
        await _collection.InsertManyAsync(invoiceSaleDetails);

    public async Task<List<InvoiceSaleDetail>> GetItemsByCajaDiaria(string companyId, string cajaDiariaId) =>
        await _collection.Find(x => x.CompanyId == companyId && x.CajaDiariaId == cajaDiariaId).ToListAsync();

    public async Task RemoveManyAsync(string companyId, string invoiceSaleId) =>
        await _collection.DeleteManyAsync(x => x.CompanyId == companyId && x.InvoiceSaleId == invoiceSaleId);
}
