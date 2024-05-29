using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Cashier.Models;
using Nebula.Common.Dto;
using Nebula.Modules.Account;

namespace Nebula.Modules.Cashier;

public interface ICajaDiariaService : ICrudOperationService<CajaDiaria>
{
    Task<List<CajaDiaria>> GetListAsync(string companyId, DateQuery query);
    Task<List<CajaDiaria>> GetCajasAbiertasAsync(string companyId);
    Task<List<CajaDiaria>> GetCajasDiariasAsync(string companyId, DateQuery query, int page = 1, int pageSize = 12);
    Task<long> GetTotalCajasDiariasAsync(string companyId, DateQuery query);
}

public class CajaDiariaService(
    MongoDatabaseService mongoDatabase,
    IInvoiceSerieService invoiceSerieService)
    : CrudOperationService<CajaDiaria>(mongoDatabase), ICajaDiariaService
{
    public async Task<List<CajaDiaria>> GetListAsync(string companyId, DateQuery query)
    {
        var filter = Builders<CajaDiaria>.Filter.And(
            Builders<CajaDiaria>.Filter.Eq(x => x.CompanyId, companyId),
            Builders<CajaDiaria>.Filter.Eq(x => x.Month, query.Month),
            Builders<CajaDiaria>.Filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<CajaDiaria>()
            .Descending("$natural")).ToListAsync();
    }

    public async Task<List<CajaDiaria>> GetCajasDiariasAsync(string companyId, DateQuery query, int page = 1,
        int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;
        var filter = Builders<CajaDiaria>.Filter.And(
            Builders<CajaDiaria>.Filter.Eq(x => x.CompanyId, companyId),
            Builders<CajaDiaria>.Filter.Eq(x => x.Month, query.Month),
            Builders<CajaDiaria>.Filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<CajaDiaria>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();
    }

    public async Task<long> GetTotalCajasDiariasAsync(string companyId, DateQuery query)
    {
        var filter = Builders<CajaDiaria>.Filter.And(
            Builders<CajaDiaria>.Filter.Eq(x => x.CompanyId, companyId),
            Builders<CajaDiaria>.Filter.Eq(x => x.Month, query.Month),
            Builders<CajaDiaria>.Filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).CountDocumentsAsync();
    }

    public override async Task<CajaDiaria> GetByIdAsync(string companyId, string id)
    {
        var filter = Builders<CajaDiaria>.Filter.And(
            Builders<CajaDiaria>.Filter.Eq(x => x.CompanyId, companyId),
            Builders<CajaDiaria>.Filter.Eq(x => x.Id, id));
        var cajaDiaria = await _collection.Find(filter).FirstOrDefaultAsync();
        var invoiceSerie = await invoiceSerieService.GetByIdAsync(companyId, cajaDiaria.InvoiceSerieId);
        cajaDiaria.WarehouseId = invoiceSerie.WarehouseId;
        return cajaDiaria;
    }

    public async Task<List<CajaDiaria>> GetCajasAbiertasAsync(string companyId)
    {
        var filter = Builders<CajaDiaria>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.Status, "ABIERTO"),
            filter.Eq(x => x.Month, DateTime.Now.ToString("MM")),
            filter.Eq(x => x.Year, DateTime.Now.ToString("yyyy")));
        return await _collection.Find(query).ToListAsync();
    }
}
