using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Sales;
using Nebula.Database.Dto.Common;
using Nebula.Database.Dto.Sales;

namespace Nebula.Database.Services.Sales;

public class InvoiceSaleService : CrudOperationService<InvoiceSale>
{
    public InvoiceSaleService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<InvoiceSale>> GetListAsync(DateQuery query)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(
            builder.Eq(x => x.Month, query.Month),
            builder.Eq(x => x.Year, query.Year),
            builder.In("DocType", new List<string>() { "BOLETA", "FACTURA" }));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<InvoiceSale>().Descending("$natural")).ToListAsync();
    }

    public async Task<InvoiceSale> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<InvoiceSale>> GetByContactIdAsync(string id, string month, string year)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.ContactId, id),
            builder.Eq(x => x.Month, month), builder.Eq(x => x.Year, year));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<InvoiceSale> SetSituacionFacturador(string id, SituacionFacturadorDto dto)
    {
        var invoiceSale = await GetByIdAsync(id);
        invoiceSale.SituacionFacturador = $"{dto.Id}:{dto.Nombre}";
        invoiceSale = await UpdateAsync(invoiceSale.Id, invoiceSale);
        return invoiceSale;
    }

    public async Task<InvoiceSale> AnularComprobante(string id)
    {
        var invoiceSale = await GetByIdAsync(id);
        invoiceSale.Anulada = true;
        invoiceSale = await UpdateAsync(invoiceSale.Id, invoiceSale);
        return invoiceSale;
    }

    public async Task<List<InvoiceSale>> GetInvoiceSalesPendingAsync()
    {
        var filter = Builders<InvoiceSale>.Filter;
        var query = filter.Eq(x => x.SituacionFacturador, "02:XML Generado");
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<List<InvoiceSale>> BusquedaAvanzadaAsync(BuscarComprobanteFormDto dto)
    {
        var filter = Builders<InvoiceSale>.Filter;
        var query = filter.And(filter.Gte(x => x.FecEmision, dto.FechaDesde),
            filter.Lte(x => x.FecEmision, dto.FechaHasta));
        if (!string.IsNullOrEmpty(dto.ContactId))
            query = filter.And(filter.Gte(x => x.FecEmision, dto.FechaDesde),
            filter.Lte(x => x.FecEmision, dto.FechaHasta), filter.Eq(x => x.ContactId, dto.ContactId));
        return await _collection.Find(query).ToListAsync();
    }
}
