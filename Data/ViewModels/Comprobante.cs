using System;
using System.Collections.Generic;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para la emisión de comprobantes.
    /// </summary>
    public class Comprobante
    {
        public int ContactId { get; set; }
        public DateTime StartDate { get; set; }
        public string DocType { get; set; }
        public string CajaId { get; set; }
        public string PaymentType { get; set; }
        public string TypeOperation { get; set; }
        public string Serie { get; set; }
        public string Number { get; set; }
        public DateTime EndDate { get; set; }
        public decimal SumTotValVenta { get; set; }
        public decimal SumTotTributos { get; set; }
        public decimal Icbper { get; set; }
        public decimal SumImpVenta { get; set; }
        public string Remark { get; set; }
        public List<DetalleComprobante> Details { get; set; }
        public List<Cuota> Cuotas { get; set; }
    }
}
