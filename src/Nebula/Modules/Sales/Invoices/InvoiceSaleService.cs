using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Sales.Models;
using Nebula.Common.Dto;
using Nebula.Modules.Sales.Invoices.Dto;
using Nebula.Modules.InvoiceHub.Dto;

namespace Nebula.Modules.Sales.Invoices;

public interface IInvoiceSaleService : ICrudOperationService<InvoiceSale>
{
    Task<List<InvoiceSale>> GetListAsync(string companyId, DateQuery query);
    Task<ResponseInvoiceSale> GetInvoiceSaleAsync(string companyId, string invoiceSaleId);
    Task<List<InvoiceSale>> GetByContactIdAsync(string companyId, string contactId, string month, string year);
    Task<List<InvoiceSale>> GetInvoicesByNumDocs(string companyId, List<string> series, List<string> numbers);
    Task<List<InvoiceSale>> GetInvoiceSaleByMonthAndYear(string companyId, string month, string year);
    Task<List<InvoiceSale>> GetInvoiceSaleByDate(string companyId, string date);
    Task<InvoiceSale> AnularComprobante(string companyId, string invoiceSaleId);
    Task<List<InvoiceSale>> GetInvoiceSalesPendingAsync(string companyId);
    Task<List<InvoiceSale>> BusquedaAvanzadaAsync(string companyId, BuscarComprobanteFormDto dto);
}

public class InvoiceSaleService : CrudOperationService<InvoiceSale>, IInvoiceSaleService
{
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly ITributoSaleService _tributoSaleService;

    public InvoiceSaleService(MongoDatabaseService mongoDatabase,
        IInvoiceSaleDetailService invoiceSaleDetailService,
        ITributoSaleService tributoSaleService) : base(mongoDatabase)
    {
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
    }

    public async Task<List<InvoiceSale>> GetListAsync(string companyId, DateQuery query)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(
            builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Month, query.Month),
            builder.Eq(x => x.Year, query.Year),
            builder.In("DocType", new List<string>() { "BOLETA", "FACTURA" }));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<InvoiceSale>().Descending("$natural"))
            .ToListAsync();
    }

    public async Task<ResponseInvoiceSale> GetInvoiceSaleAsync(string companyId, string invoiceSaleId)
    {
        var invoiceSale = await GetByIdAsync(companyId, invoiceSaleId);
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(companyId, invoiceSale.Id);
        var tributoSales = await _tributoSaleService.GetListAsync(companyId, invoiceSale.Id);
        return new ResponseInvoiceSale()
        {
            InvoiceSale = invoiceSale,
            InvoiceSaleDetails = invoiceSaleDetails,
            TributoSales = tributoSales
        };
    }

    public async Task<List<InvoiceSale>> GetByContactIdAsync(string companyId, string contactId, string month, string year)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Month, month), builder.Eq(x => x.Year, year));
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Obtener comprobantes por serie y números.
    /// </summary>
    /// <param name="series">series de los comprobantes</param>
    /// <param name="numbers">Lista de números de comprobantes</param>
    /// <returns>Lista de Comprobantes</returns>
    public async Task<List<InvoiceSale>> GetInvoicesByNumDocs(string companyId, List<string> series, List<string> numbers)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.In(x => x.Serie, series), builder.In(x => x.Number, numbers));
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Obtener lista de comprobantes por mes y año.
    /// </summary>
    /// <param name="month">mes</param>
    /// <param name="year">año</param>
    /// <returns>Lista de comprobantes</returns>
    public async Task<List<InvoiceSale>> GetInvoiceSaleByMonthAndYear(string companyId, string month, string year)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Month, month), builder.Eq(x => x.Year, year));
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Obtener lista de comprobantes de una fecha especifica.
    /// </summary>
    /// <param name="date">fecha de emisión</param>
    /// <returns>lista de comprobantes</returns>
    public async Task<List<InvoiceSale>> GetInvoiceSaleByDate(string companyId, string date)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.FecEmision, date));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<InvoiceSale> SetStatusFacturador(InvoiceSale invoice, BillingResponse billing)
    {

        var invoiceSale = await UpdateAsync(invoice.Id, invoice);
        return invoiceSale;
    }

    public async Task<InvoiceSale> AnularComprobante(string companyId, string invoiceSaleId)
    {
        var invoiceSale = await GetByIdAsync(companyId, invoiceSaleId);
        invoiceSale.Anulada = true;
        invoiceSale = await UpdateAsync(invoiceSale.Id, invoiceSale);
        return invoiceSale;
    }

    public async Task<List<InvoiceSale>> GetInvoiceSalesPendingAsync(string companyId)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Not(builder.Eq(x => x.DocType, "NOTA")),
            builder.Eq(x => x.BillingResponse.Success, false));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<InvoiceSale>> BusquedaAvanzadaAsync(string companyId, BuscarComprobanteFormDto dto)
    {
        var filter = Builders<InvoiceSale>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Gte(x => x.FecEmision, dto.FechaDesde),
            filter.Lte(x => x.FecEmision, dto.FechaHasta));
        if (!string.IsNullOrEmpty(dto.ContactId))
            query = filter.And(filter.Gte(x => x.FecEmision, dto.FechaDesde),
                filter.Lte(x => x.FecEmision, dto.FechaHasta), filter.Eq(x => x.ContactId, dto.ContactId));
        return await _collection.Find(query).ToListAsync();
    }

}
