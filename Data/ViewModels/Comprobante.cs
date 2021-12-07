using System;
using System.Collections.Generic;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para la emisión de comprobantes.
    /// </summary>
    public class Comprobante
    {
        /// <summary>
        /// Id de contacto.
        /// </summary>
        public int ContactId { get; set; }

        /// <summary>
        /// Fecha de registro.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Tipo de documento.
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Id de Caja diaria.
        /// </summary>
        public int CajaDiariaId { get; set; }

        /// <summary>
        /// Forma de Pago.
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// Tipo Operación.
        /// </summary>
        public string TypeOperation { get; set; }

        /// <summary>
        /// Serie Comprobante.
        /// </summary>
        public string Serie { get; set; }

        /// <summary>
        /// Número Comprobante.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Fecha Vencimiento.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// SubTotal.
        /// </summary>
        public decimal SumTotValVenta { get; set; }

        /// <summary>
        /// Total Tributos.
        /// </summary>
        public decimal SumTotTributos { get; set; }

        /// <summary>
        /// Impuesto a la Bolsa plástica.
        /// </summary>
        public decimal Icbper { get; set; }

        /// <summary>
        /// Importe Total a Pagar.
        /// </summary>
        public decimal SumImpVenta { get; set; }

        /// <summary>
        /// Observación o Comentario.
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Tipo Comprobante.
        /// </summary>
        public string InvoiceType { get; set; }

        /// <summary>
        /// Detalle Comprobante.
        /// </summary>
        public List<DetalleComprobante> Details { get; set; }

        /// <summary>
        /// Cuotas a Crédito.
        /// </summary>
        public List<Cuota> Cuotas { get; set; }
    }
}
