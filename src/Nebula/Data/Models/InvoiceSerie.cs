namespace Nebula.Data.Models
{
    /// <summary>
    /// Series de facturación.
    /// </summary>
    public class InvoiceSerie
    {
        public string Id { get; set; }

        /// <summary>
        /// Identificador Serie.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identificar Almacén.
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// Serie Factura.
        /// </summary>
        public string Factura { get; set; }

        /// <summary>
        /// Contador Factura.
        /// </summary>
        public int CounterFactura { get; set; }

        /// <summary>
        /// Serie Boleta.
        /// </summary>
        public string Boleta { get; set; }

        /// <summary>
        /// Contador Boleta.
        /// </summary>
        public int CounterBoleta { get; set; }

        /// <summary>
        /// Serie Nota de Venta.
        /// </summary>
        public string NotaDeVenta { get; set; }

        /// <summary>
        /// Contador Nota de Venta.
        /// </summary>
        public int CounterNotaDeVenta { get; set; }
    }
}
