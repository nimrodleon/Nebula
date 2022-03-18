using System.Text.Json;
using CpeLibPE.Facturador;
using Nebula.Data.Helpers;
using Nebula.Data.Models;
using Raven.Client.Documents;
using sfs = CpeLibPE.Facturador.Models;

namespace Nebula.Data.Services;

public interface ICpeService
{
    public Task<bool> CreateBoletaJson(string invoice);
    public Task<bool> CreateFacturaJson(string invoice);
}

/// <summary>
/// Generar Comprobante Electrónico.
/// </summary>
public class CpeService : ICpeService
{
    private readonly ILogger _logger;
    private readonly IRavenDbContext _context;
    private Configuration _configuration;

    public CpeService(ILogger<CpeService> logger, IRavenDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Cargar configuración del Sistema.
    /// </summary>
    private async Task GetConfiguration()
    {
        using var session = _context.Store.OpenAsyncSession();
        _configuration = await session.LoadAsync<Configuration>("default");
        _logger.LogInformation($"Configuración: {JsonSerializer.Serialize(_configuration)}");
    }

    /// <summary>
    /// Crear Archivo Json Boleta.
    /// </summary>
    public async Task<bool> CreateBoletaJson(string id)
    {
        await GetConfiguration();
        using var session = _context.Store.OpenAsyncSession();
        _logger.LogInformation("*** CreateBoletaJson ***");
        var invoiceSale = await session.LoadAsync<InvoiceSale>(id);
        var invoiceSaleDetails = await session.Query<InvoiceSaleDetail>()
            .Where(m => m.InvoiceSale == id).ToListAsync();
        var tributoSales = await session.Query<TributoSale>()
            .Where(m => m.InvoiceSale == id).ToListAsync();
        _logger.LogInformation(JsonSerializer.Serialize(invoiceSale));
        var cabecera = GetInvoice(invoiceSale);
        var detalle = GetInvoiceDetail(invoiceSaleDetails);
        var tributos = GetTributos(tributoSales);
        var leyendas = GetLeyendas(tributoSales, invoiceSale);

        // Configurar estructura de la boleta.
        var boleta = new JsonBoletaParser()
        {
            cabecera = cabecera,
            detalle = detalle,
            tributos = tributos,
            leyendas = leyendas
        };

        // Información del comprobante.
        _logger.LogInformation(JsonSerializer.Serialize(boleta));

        // Escribir datos en el Disco duro.
        string fileName = Path.Combine("DATA",
            $"{_configuration.Ruc}-03-{invoiceSale.Serie}-{invoiceSale.Number}.json");
        boleta.CreateJson(Path.Combine(_configuration.FileSunat, fileName));
        return File.Exists(Path.Combine(_configuration.FileSunat, fileName));
    }

    /// <summary>
    /// Crear Archivo Json Factura.
    /// </summary>
    /// <param name="id">ID del comprobante de venta.</param>
    public async Task<bool> CreateFacturaJson(string id)
    {
        await GetConfiguration();
        using var session = _context.Store.OpenAsyncSession();
        _logger.LogInformation("*** CreateFacturaJson ***");
        var invoiceSale = await session.LoadAsync<InvoiceSale>(id);
        var invoiceSaleDetails = await session.Query<InvoiceSaleDetail>()
            .Where(m => m.InvoiceSale == id).ToListAsync();
        var tributoSales = await session.Query<TributoSale>()
            .Where(m => m.InvoiceSale == id).ToListAsync();
        var invoiceSaleAccounts = await session.Query<InvoiceSaleAccount>()
            .Where(m => m.InvoiceSale == id).ToListAsync();
        _logger.LogInformation(JsonSerializer.Serialize(invoiceSale));
        var cabecera = GetInvoice(invoiceSale);
        var detalle = GetInvoiceDetail(invoiceSaleDetails);
        var tributos = GetTributos(tributoSales);
        var leyendas = GetLeyendas(tributoSales, invoiceSale);
        var datoPago = GetDatoPago(invoiceSale);
        var detallePagos = new List<sfs.DetallePago>();
        if (invoiceSale.FormaPago.Equals("Credito"))
            detallePagos = GetDetallePagos(invoiceSaleAccounts, invoiceSale.TipMoneda);

        // Configurar estructura de la boleta.
        var factura = new JsonFacturaParser()
        {
            cabecera = cabecera,
            detalle = detalle,
            tributos = tributos,
            leyendas = leyendas,
            datoPago = datoPago,
            detallePago = detallePagos
        };

        // Información del comprobante.
        _logger.LogInformation(JsonSerializer.Serialize(factura));

        // Escribir datos en el Disco duro.
        string fileName = Path.Combine("DATA",
            $"{_configuration.Ruc}-01-{invoiceSale.Serie}-{invoiceSale.Number}.json");
        factura.CreateJson(Path.Combine(_configuration.FileSunat, fileName));
        return File.Exists(Path.Combine(_configuration.FileSunat, fileName));
    }

    /// <summary>
    /// Comprobar si Existe operaciones gratuitas.
    /// </summary>
    private bool ExistFreeOperations(List<TributoSale> tributos)
    {
        bool result = false;
        tributos.ForEach(item => result = item.IdeTributo.Equals("9996"));
        return result;
    }

    /// <summary>
    /// Configurar cabecera del comprobante.
    /// </summary>
    private sfs.Invoice GetInvoice(InvoiceSale invoiceSale)
    {
        return new sfs.Invoice()
        {
            tipOperacion = invoiceSale.TipOperacion,
            fecEmision = invoiceSale.FecEmision,
            horEmision = invoiceSale.HorEmision,
            fecVencimiento = invoiceSale.FecVencimiento,
            codLocalEmisor = _configuration.CodLocalEmisor,
            tipDocUsuario = invoiceSale.TipDocUsuario,
            numDocUsuario = invoiceSale.NumDocUsuario,
            rznSocialUsuario = invoiceSale.RznSocialUsuario,
            tipMoneda = invoiceSale.TipMoneda,
            sumTotTributos = Convert.ToDecimal(invoiceSale.SumTotTributos).ToString("N2"),
            sumTotValVenta = Convert.ToDecimal(invoiceSale.SumTotValVenta).ToString("N2"),
            sumPrecioVenta = Convert.ToDecimal(invoiceSale.SumImpVenta).ToString("N2"),
            sumImpVenta = Convert.ToDecimal(invoiceSale.SumImpVenta).ToString("N2"),
        };
    }

    /// <summary>
    /// Configurar detalle del comprobante.
    /// </summary>
    private List<sfs.InvoiceDetail> GetInvoiceDetail(List<InvoiceSaleDetail> details)
    {
        var detalle = new List<sfs.InvoiceDetail>();
        details.ForEach(item =>
        {
            detalle.Add(new sfs.InvoiceDetail()
            {
                codUnidadMedida = item.CodUnidadMedida,
                ctdUnidadItem = item.CtdUnidadItem.ToString(),
                codProducto = item.CodProducto,
                codProductoSUNAT = item.CodProductoSunat,
                desItem = item.DesItem,
                mtoValorUnitario = Convert.ToDecimal(item.MtoValorUnitario).ToString("N2"),
                sumTotTributosItem = Convert.ToDecimal(item.SumTotTributosItem).ToString("N2"),
                // Tributo: IGV(1000).
                codTriIGV = item.CodTriIgv,
                mtoIgvItem = Convert.ToDecimal(item.MtoIgvItem).ToString("N2"),
                mtoBaseIgvItem = Convert.ToDecimal(item.MtoBaseIgvItem).ToString("N2"),
                nomTributoIgvItem = item.NomTributoIgvItem,
                codTipTributoIgvItem = item.CodTipTributoIgvItem,
                tipAfeIGV = item.TipAfeIgv,
                porIgvItem = item.PorIgvItem,
                // Tributo ICBPER 7152.
                codTriIcbper = item.CodTriIcbper,
                mtoTriIcbperItem = Convert.ToDecimal(item.MtoTriIcbperItem).ToString("N2"),
                ctdBolsasTriIcbperItem = item.CtdBolsasTriIcbperItem.ToString(),
                nomTributoIcbperItem = item.NomTributoIcbperItem,
                codTipTributoIcbperItem = item.CodTipTributoIcbperItem,
                mtoTriIcbperUnidad = Convert.ToDecimal(item.MtoTriIcbperItem).ToString("N2"),
                // Importe de Venta.
                mtoPrecioVentaUnitario = Convert.ToDecimal(item.MtoPrecioVentaUnitario).ToString("N2"),
                mtoValorVentaItem = Convert.ToDecimal(item.MtoValorVentaItem).ToString("N2")
            });
        });
        return detalle;
    }

    /// <summary>
    /// Configurar Tributos del Comprobante.
    /// </summary>
    private List<sfs.Tributo> GetTributos(List<TributoSale> tributos)
    {
        var result = new List<sfs.Tributo>();
        tributos.ForEach(item =>
        {
            result.Add(new sfs.Tributo()
            {
                ideTributo = item.IdeTributo,
                nomTributo = item.NomTributo,
                codTipTributo = item.CodTipTributo,
                mtoBaseImponible = Convert.ToDecimal(item.MtoBaseImponible).ToString("N2"),
                mtoTributo = Convert.ToDecimal(item.MtoTributo).ToString("N2")
            });
        });
        return result;
    }

    /// <summary>
    /// Configurar Leyendas del Comprobante.
    /// </summary>
    private List<sfs.Leyenda> GetLeyendas(
        List<TributoSale> tributoSales, InvoiceSale invoiceSale)
    {
        var leyendas = new List<sfs.Leyenda>();
        if (ExistFreeOperations(tributoSales))
        {
            leyendas.Add(new sfs.Leyenda()
            {
                codLeyenda = "1002",
                desLeyenda = "TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE"
            });
        }

        leyendas.Add(new sfs.Leyenda()
        {
            codLeyenda = "1000",
            desLeyenda = new NumberToLetters(Convert.ToDecimal(invoiceSale.SumImpVenta)).ToString()
        });
        return leyendas;
    }

    /// <summary>
    /// Configurar forma de pago.
    /// </summary>
    private sfs.DatoPago GetDatoPago(InvoiceSale invoiceSale)
    {
        return new sfs.DatoPago()
        {
            formaPago = invoiceSale.FormaPago,
            mtoNetoPendientePago = Convert.ToDecimal(invoiceSale.SumImpVenta).ToString("N2"),
            tipMonedaMtoNetoPendientePago = invoiceSale.TipMoneda
        };
    }

    /// <summary>
    /// Configurar Cuentas por cobrar.
    /// </summary>
    private List<sfs.DetallePago> GetDetallePagos(
        List<InvoiceSaleAccount> invoiceSaleAccounts, string tipMoneda)
    {
        var detallePagos = new List<sfs.DetallePago>();
        invoiceSaleAccounts.OrderBy(m => m.Cuota).ToList()
            .ForEach(item =>
            {
                detallePagos.Add(new sfs.DetallePago()
                {
                    mtoCuotaPago = Convert.ToDecimal(item.Amount).ToString("N2"),
                    fecCuotaPago = item.EndDate,
                    tipMonedaCuotaPago = tipMoneda,
                });
            });
        return detallePagos;
    }
}
