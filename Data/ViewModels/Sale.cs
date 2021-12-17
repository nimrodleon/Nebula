using System.Collections.Generic;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para el punto de venta.
    /// </summary>
    public class Sale
    {
        /// <summary>
        /// Id del Contacto.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Medios de Pago.
        /// </summary>
        public int PaymentMethod { get; set; }

        /// <summary>
        /// Tipo documento.
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Monto Cobrado.
        /// </summary>
        public decimal? MontoTotal { get; set; }

        /// <summary>
        /// Vuelto para el Cliente.
        /// </summary>
        public decimal? Vuelto { get; set; }

        /// <summary>
        /// SubTotal.
        /// </summary>
        public decimal? SumTotValVenta { get; set; }

        /// <summary>
        /// Sumatoria Tributos.
        /// </summary>
        public decimal? SumTotTributos { get; set; }

        /// <summary>
        /// Importe a Cobrar.
        /// </summary>
        public decimal? SumImpVenta { get; set; }

        /// <summary>
        /// Observación.
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Detalle de Venta.
        /// </summary>
        public List<SaleDetail> Details { get; set; }
    }
}
