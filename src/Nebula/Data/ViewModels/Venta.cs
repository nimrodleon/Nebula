using Nebula.Data.Models;
using Nebula.Data.Models.Common;
using Nebula.Data.Models.Sales;

namespace Nebula.Data.ViewModels;

/// <summary>
/// modelo para el punto de venta.
/// </summary>
public class Venta : CpeBase
{
    /// <summary>
    /// Medios de Pago.
    /// </summary>
    public string FormaPago { get; set; }

    /// <summary>
    /// Monto Cobrado.
    /// </summary>
    public decimal MontoTotal { get; set; }

    /// <summary>
    /// Vuelto para el Cliente.
    /// </summary>
    public decimal Vuelto { get; set; }

    /// <summary>
    /// Configurar cabecera de venta.
    /// </summary>
    /// <param name="configuration">Configuración del sistema</param>
    /// <param name="contact">Información de contacto</param>
    public InvoiceSale GetInvoice(Configuration configuration, Contact contact)
    {
        CalcImporteVenta();
        // Devolver Configuración comprobante de venta.
        return new InvoiceSale()
        {
            Id = string.Empty,
            DocType = DocType,
            TipOperacion = "0101",
            FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
            HorEmision = DateTime.Now.ToString("HH:mm:ss"),
            CodLocalEmisor = configuration.CodLocalEmisor,
            FormaPago = FormaPago,
            ContactId = contact.Id,
            TipDocUsuario = contact.DocType,
            NumDocUsuario = contact.Document,
            RznSocialUsuario = contact.Name,
            TipMoneda = configuration.TipMoneda,
            SumTotValVenta = SumTotValVenta,
            SumTotTributos = SumTotTributos,
            SumImpVenta = SumImpVenta,
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
        };
    }
}
