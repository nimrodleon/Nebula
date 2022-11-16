using Nebula.Database.Services.Common;
using Nebula.Database.Services.Sales;

namespace Nebula.Database.Services.Facturador;

public class FacturadorService
{
    private readonly ConfigurationService _configurationService;
    private readonly InvoiceSaleService _invoiceSaleService;
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;
    private readonly TributoSaleService _tributoSaleService;

    public FacturadorService(ConfigurationService configurationService,
        InvoiceSaleService invoiceSaleService, InvoiceSaleDetailService invoiceSaleDetailService,
        TributoSaleService tributoSaleService)
    {
        _configurationService = configurationService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
        _tributoSaleService = tributoSaleService;
    }

    /// <summary>
    /// Devolver Boleta/Factura Registrada.
    /// </summary>
    /// <param name="invoiceSaleId">Id Venta</param>
    /// <returns>InvoiceSaleDto</returns>
    private async Task<InvoiceSaleDto> GetInvoiceSaleDto(string invoiceSaleId)
    {
        var invoiceSale = await _invoiceSaleService.GetAsync(invoiceSaleId);
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(invoiceSaleId);
        var tributoSales = await _tributoSaleService.GetListAsync(invoiceSaleId);
        return new InvoiceSaleDto
        {
            InvoiceSale = invoiceSale,
            InvoiceSaleDetails = invoiceSaleDetails,
            TributoSales = tributoSales
        };
    }

    /// <summary>
    /// Generar Boleta/Factura en la carpeta DATA de Facturador.
    /// </summary>
    /// <returns>Existencia del Archivo.</returns>
    public async Task<bool> JsonInvoiceParser(string invoiceSaleId)
    {
        var configuration = await _configurationService.GetAsync();
        var invoiceSaleDto = await GetInvoiceSaleDto(invoiceSaleId);
        // configurar nombre del archivo.
        string typeDoc = string.Empty;
        if (invoiceSaleDto.InvoiceSale.DocType == "BOLETA") typeDoc = "03";
        if (invoiceSaleDto.InvoiceSale.DocType == "FACTURA") typeDoc = "01";
        string fileName = $"{configuration.Ruc}-{typeDoc}-{invoiceSaleDto.InvoiceSale.Serie}-{invoiceSaleDto.InvoiceSale.Number}.json";
        // generar archivo json.
        string pathBase = Path.Combine(configuration.FileSunat, "sfs");
        string pathData = Path.Combine(pathBase, "DATA");
        string pathFile = Path.Combine(pathData, fileName);
        if (invoiceSaleDto.InvoiceSale.DocType == "BOLETA")
        {
            var boletaParser = new JsonBoletaParser(invoiceSaleDto);
            boletaParser.CreateJson(pathFile);
        }

        if (invoiceSaleDto.InvoiceSale.DocType == "FACTURA")
        {
            var facturaParser = new JsonFacturaParser(invoiceSaleDto);
            facturaParser.CreateJson(pathFile);
        }

        return File.Exists(pathFile);
    }
}
