using System;
using Nebula.Data.Models;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para la emisión de comprobantes.
    /// </summary>
    public class Comprobante : CpeBase
    {
        /// <summary>
        /// Fecha de registro.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Forma de Pago #Credito/Contado.
        /// </summary>
        public string FormaPago { get; set; }

        /// <summary>
        /// Tipo Operación.
        /// </summary>
        public string TypeOperation { get; set; }

        /// <summary>
        /// Serie Comprobante #SOLO PARA COMPRAS.
        /// </summary>
        public string Serie { get; set; }

        /// <summary>
        /// Número Comprobante #SOLO PARA COMPRAS.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Fecha Vencimiento.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Configurar cabecera de venta.
        /// </summary>
        public Invoice GetInvoice(Configuration config, Contact client)
        {
            CalcImporteVenta();

            // configurar fecha de registro.
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string startDate = DateTime.Now.ToString("yyyy-MM-dd");
            string startTime = DateTime.Now.ToString("HH:mm:ss");
            string fecVencimiento = "-";
            if (FormaPago.Equals("Credito"))
                if (EndDate != null)
                    fecVencimiento = Convert.ToDateTime(EndDate).ToString("yyyy-MM-dd");
            if (InvoiceType.Equals("COMPRA"))
            {
                year = StartDate.ToString("yyyy");
                month = StartDate.ToString("MM");
                startDate = StartDate.ToString("yyyy-MM-dd");
                startTime = "00:00:00";
            }

            // Devolver Configuración Factura.
            var invoice = new Invoice()
            {
                DocType = DocType,
                TipOperacion = TypeOperation,
                FecEmision = startDate,
                HorEmision = startTime,
                FecVencimiento = fecVencimiento,
                CodLocalEmisor = config.CodLocalEmisor,
                FormaPago = FormaPago,
                ContactId = client.Id,
                TipDocUsuario = client.DocType.ToString(),
                NumDocUsuario = client.Document,
                RznSocialUsuario = client.Name,
                TipMoneda = config.TipMoneda,
                SumTotValVenta = SumTotValVenta,
                SumTotTributos = SumTotTributos,
                SumImpVenta = SumImpVenta,
                InvoiceType = InvoiceType,
                Year = year,
                Month = month,
            };

            // configurar número y serie de facturación.
            if (InvoiceType.Equals("COMPRA"))
            {
                invoice.Number = Number;
                invoice.Serie = Serie;
            }

            return invoice;
        }
    }
}
