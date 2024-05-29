using Microsoft.Extensions.Options;
using Nebula.Modules.Account.Models;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Invoices;

public interface IInvoiceSaleFileService
{
    FileStream GetXml(Company company, InvoiceSale invoice);
}

public class InvoiceSaleFileService(
    IOptions<InvoiceHubSettings> settings,
    ILogger<InvoiceSaleFileService> logger)
    : IInvoiceSaleFileService
{
    private readonly InvoiceHubSettings _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    public FileStream GetXml(Company company, InvoiceSale invoice)
    {
        string storatePath = _settings.StoragePath;
        string dirXml = Path.Combine(storatePath, company.Id, invoice.Year, invoice.Month);
        string fileXml = Path.Combine(dirXml, $"{company.Ruc}-{invoice.TipoDoc}-{invoice.Serie}-{invoice.Correlativo}.xml");
        logger.LogInformation("GetXML: " + fileXml);
        return new FileStream(fileXml, FileMode.Open);
    }
}
