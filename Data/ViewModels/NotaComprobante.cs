using System;
using System.Collections.Generic;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para la emisión de notas crédito/débito.
    /// </summary>
    public class NotaComprobante
    {
        /// <summary>
        /// Id del Comprobante.
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Fecha de registro.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Tipo documento NOTA: (CRÉDITO/DÉBITO) => (NC/ND).
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Código del Motivo de emisión.
        /// </summary>
        public string CodMotivo { get; set; }

        /// <summary>
        /// Serie Comprobante #SOLO PARA COMPRAS.
        /// </summary>
        public string Serie { get; set; }

        /// <summary>
        /// Número Comprobante #SOLO PARA COMPRAS.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Descripción del motivo.
        /// </summary>
        public string DesMotivo { get; set; }

        /// <summary>
        /// SubTotal.
        /// </summary>
        public decimal SumTotValVenta { get; set; }

        /// <summary>
        /// Sumatoria Tributos.
        /// </summary>
        public decimal SumTotTributos { get; set; }

        /// <summary>
        /// Importe a Cobrar.
        /// </summary>
        public decimal SumImpVenta { get; set; }

        /// <summary>
        /// Detalle de Venta.
        /// </summary>
        public List<CpeDetail> Details { get; set; }
    }
}
