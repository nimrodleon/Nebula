using Nebula.Data.Models;
using Nebula.Data.ViewModels;

namespace Nebula.Data.Services;

public interface ISaleService
{
    public void SetModel(Venta model);
    public Task<InvoiceSale> CreateQuickSale(string cajaDiaria);
}

/// <summary>
/// Servicio para gestionar ventas.
/// </summary>
public class SaleService : ISaleService
{
    private readonly IRavenDbContext _context;
    private Configuration _configuration;
    private Contact _contact;
    private Venta _venta;

    public SaleService(IRavenDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Establecer modelo venta.
    /// </summary>
    public void SetModel(Venta model) => _venta = model;

    /// <summary>
    /// Guardar comprobante de venta rápida.
    /// </summary>
    /// <param name="cajaDiariaId">ID caja diaria.</param>
    public async Task<InvoiceSale> CreateQuickSale(string cajaDiariaId)
    {
        await GetConfiguration();
        await GetContact(_venta.ContactId);
        using var session = _context.Store.OpenAsyncSession();
        // Cambiar el número máximo de solicitudes en una sola sesión.
        session.Advanced.MaxNumberOfRequestsPerSession = 1000;
        var invoiceSale = _venta.GetInvoice(_configuration, _contact);
        var cajaDiaria = await session.LoadAsync<CajaDiaria>(cajaDiariaId);
        var invoiceSerie = await session.LoadAsync<InvoiceSerie>(cajaDiaria.Terminal.Split(":")[0].Trim());
        GenerateInvoiceSerie(ref invoiceSerie, ref invoiceSale, _venta.DocType);
        // Agregar Información del comprobante.
        await session.StoreAsync(invoiceSale);
        await session.SaveChangesAsync();

        // Agregar detalles del comprobante.
        var invoiceSaleDetails = _venta.GetInvoiceDetail(invoiceSale.Id);
        foreach (var item in invoiceSaleDetails) await session.StoreAsync(item);
        await session.SaveChangesAsync();

        // Agregar Tributos de Factura.
        var tributoSales = _venta.GetTributo(invoiceSale.Id);
        foreach (var item in tributoSales) await session.StoreAsync(item);
        await session.SaveChangesAsync();

        // Registrar operación de Caja.
        var cashierDetail = new CashierDetail()
        {
            Id = string.Empty,
            CajaDiaria = cajaDiariaId,
            Document = $"{invoiceSale.Serie}-{invoiceSale.Number}",
            Contact = invoiceSale.RznSocialUsuario,
            Remark = _venta.Remark,
            Type = "ENTRADA",
            TypeOperation = TypeOperation.Comprobante,
            FormaPago = _venta.FormaPago,
            Amount = invoiceSale.SumImpVenta
        };
        await session.StoreAsync(cashierDetail);
        await session.SaveChangesAsync();

        return invoiceSale;
    }

    /// <summary>
    ///  Carga la configuración del sistema.
    /// </summary>
    private async Task GetConfiguration()
    {
        using var session = _context.Store.OpenAsyncSession();
        _configuration = await session.LoadAsync<Configuration>("default");
    }

    /// <summary>
    /// Carga los datos de contacto.
    /// </summary>
    /// <param name="id">ID de contacto.</param>
    private async Task GetContact(string id)
    {
        using var session = _context.Store.OpenAsyncSession();
        _contact = await session.LoadAsync<Contact>(id);
    }

    /// <summary>
    /// Generar serie y número del comprobante de venta.
    /// </summary>
    /// <param name="invoiceSerie">Series de facturación</param>
    /// <param name="invoiceSale">Modelo de Facturación</param>
    /// <param name="docType">Tipo comprobante de venta</param>
    private void GenerateInvoiceSerie(ref InvoiceSerie invoiceSerie, ref InvoiceSale invoiceSale, string docType)
    {
        int numComprobante = 0;
        string THROW_MESSAGE = "Ingresa serie de comprobante!";
        switch (docType)
        {
            case "FACTURA":
                invoiceSale.Serie = invoiceSerie.Factura;
                numComprobante = invoiceSerie.CounterFactura;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante = numComprobante + 1;
                invoiceSerie.CounterFactura = numComprobante;
                break;
            case "BOLETA":
                invoiceSale.Serie = invoiceSerie.Boleta;
                numComprobante = invoiceSerie.CounterBoleta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante = numComprobante + 1;
                invoiceSerie.CounterBoleta = numComprobante;
                break;
            case "NOTA":
                invoiceSale.Serie = invoiceSerie.NotaDeVenta;
                numComprobante = invoiceSerie.CounterNotaDeVenta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante = numComprobante + 1;
                invoiceSerie.CounterNotaDeVenta = numComprobante;
                break;
        }

        invoiceSale.Number = numComprobante.ToString("D8");
    }
}
