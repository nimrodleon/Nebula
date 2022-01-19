using System;
using Nebula.Data.Models;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para el punto de venta.
    /// </summary>
    public class Venta : CpeBase
    {
        /// <summary>
        /// Medios de Pago.
        /// </summary>
        public int PaymentMethod { get; set; }

        /// <summary>
        /// Monto Cobrado.
        /// </summary>
        public decimal? MontoTotal { get; set; }

        /// <summary>
        /// Vuelto para el Cliente.
        /// </summary>
        public decimal? Vuelto { get; set; }

        /// <summary>
        /// Configurar cabecera de venta.
        /// </summary>
        public Invoice GetInvoice(Configuration config, Contact client)
        {
            CalcImporteVenta();
            // Devolver Configuración Factura.
            return new Invoice()
            {
                DocType = DocType,
                TipOperacion = "0101",
                FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
                HorEmision = DateTime.Now.ToString("HH:mm:ss"),
                CodLocalEmisor = config.CodLocalEmisor,
                FormaPago = "Contado",
                ContactId = client.Id,
                TipDocUsuario = client.DocType.ToString(),
                NumDocUsuario = client.Document,
                RznSocialUsuario = client.Name,
                TipMoneda = config.TipMoneda,
                SumTotValVenta = SumTotValVenta,
                SumTotTributos = SumTotTributos,
                SumImpVenta = SumImpVenta,
                InvoiceType = "VENTA",
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM"),
            };
        }
    }
}
