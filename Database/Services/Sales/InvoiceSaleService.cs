using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Sales;
using Nebula.Database.Dto.Common;
using Nebula.Database.Dto.Sales;
using Nebula.Database.Helpers;
using Nebula.Database.Models.Common;
using Nebula.Database.Services.Common;

namespace Nebula.Database.Services.Sales;

public class InvoiceSaleService : CrudOperationService<InvoiceSale>
{
    private readonly IConfiguration _configuration;
    private readonly ConfigurationService _configurationService;

    public InvoiceSaleService(IOptions<DatabaseSettings> options,
        IConfiguration configuration, ConfigurationService configurationService) : base(options)
    {
        _configuration = configuration;
        _configurationService = configurationService;
    }

    public async Task<List<InvoiceSale>> GetListAsync(DateQuery query)
    {
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(
            builder.Eq(x => x.Month, query.Month),
            builder.Eq(x => x.Year, query.Year),
            builder.In("DocType", new List<string>() { "BOLETA", "FACTURA" }));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<InvoiceSale>().Descending("$natural"))
            .ToListAsync();
    }

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
        var builder = Builders<InvoiceSale>.Filter;
        var filter = builder.And(builder.Not(builder.Eq(x => x.DocType, "NOTA")),
            builder.Nin("SituacionFacturador", new List<string>()
            {
                "03:Enviado y Aceptado SUNAT", "04:Enviado y Aceptado SUNAT con Obs.",
                "11:Enviado y Aceptado SUNAT", "12:Enviado y Aceptado SUNAT con Obs.",
            }));
        return await _collection.Find(filter).ToListAsync();
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

    /// <summary>
    /// Datos devueltos para imprimir el ticket del documento.
    /// </summary>
    /// <param name="invoiceId">Id del Comprobante</param>
    /// <returns>TicketDto</returns>
    public async Task<TicketDto> GetTicketDto(string invoiceId)
    {
        var invoice = await GetByIdAsync(invoiceId);
        var configuration = await _configurationService.GetAsync();
        string tipDocu = string.Empty;
        if (invoice.DocType.Equals("FACTURA")) tipDocu = "01";
        if (invoice.DocType.Equals("BOLETA")) tipDocu = "03";
        // 20520485750-03-B001-00000015
        string nomArch = $"{configuration.Ruc}-{tipDocu}-{invoice.Serie}-{invoice.Number}.xml";
        string pathXml = string.Empty;
        var storagePath = _configuration.GetValue<string>("StoragePath");
        // abrir en la ruta del facturador.
        if (invoice.DocumentPath == DocumentPathType.SFS)
        {
            string? sunatArchivos = _configuration.GetValue<string>("sunatArchivos");
            if (sunatArchivos is null) sunatArchivos = string.Empty;
            string carpetaArchivoSunat = Path.Combine(sunatArchivos, "sfs");
            pathXml = Path.Combine(carpetaArchivoSunat, "FIRMA", nomArch);
        }

        // abrir en la ruta de la carpeta control.
        if (invoice.DocumentPath == DocumentPathType.CONTROL)
        {
            if (storagePath is null) storagePath = string.Empty;
            string carpetaArchivoSunat = Path.Combine(storagePath, "sunat");
            string carpetaRepo = Path.Combine(carpetaArchivoSunat, "FIRMA", invoice.Year, invoice.Month);
            pathXml = Path.Combine(carpetaRepo, nomArch);
        }

        TicketDto ticket = new TicketDto
        {
            Configuration = configuration,
            InvoiceSale = invoice
        };
        LeerDigestValue digest = new LeerDigestValue();
        ticket.DigestValue = digest.GetValue(pathXml);
        return ticket;
    }
}
