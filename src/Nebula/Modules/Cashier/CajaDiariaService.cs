using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Cashier.Models;
using Nebula.Common.Dto;
using Nebula.Modules.Configurations.Warehouses;

namespace Nebula.Modules.Cashier;

public interface ICajaDiariaService : ICrudOperationService<CajaDiaria>
{
    Task<List<CajaDiaria>> GetListAsync(DateQuery query);
    Task<List<CajaDiaria>> GetCajasAbiertasAsync();
}

public class CajaDiariaService : CrudOperationService<CajaDiaria>, ICajaDiariaService
{
    private readonly IInvoiceSerieService _invoiceSerieService;

    public CajaDiariaService(IOptions<DatabaseSettings> options,
        IInvoiceSerieService invoiceSerieService) : base(options)
    {
        _invoiceSerieService = invoiceSerieService;
    }

    public async Task<List<CajaDiaria>> GetListAsync(DateQuery query)
    {
        var filter = Builders<CajaDiaria>.Filter.And(
            Builders<CajaDiaria>.Filter.Eq(x => x.Month, query.Month),
            Builders<CajaDiaria>.Filter.Eq(x => x.Year, query.Year));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<CajaDiaria>()
            .Descending("$natural")).ToListAsync();
    }

    public override async Task<CajaDiaria> GetByIdAsync(string id)
    {
        var cajaDiaria = await base.GetByIdAsync(id);
        var invoiceSerie = await _invoiceSerieService.GetByIdAsync(cajaDiaria.InvoiceSerie);
        cajaDiaria.WarehouseId = invoiceSerie.WarehouseId;
        return cajaDiaria;
    }

    public async Task<List<CajaDiaria>> GetCajasAbiertasAsync()
    {
        var filter = Builders<CajaDiaria>.Filter;
        var query = filter.And(filter.Eq(x => x.Status, "ABIERTO"),
            filter.Eq(x => x.Month, DateTime.Now.ToString("MM")),
            filter.Eq(x => x.Year, DateTime.Now.ToString("yyyy")));
        return await _collection.Find(query).ToListAsync();
    }
}
