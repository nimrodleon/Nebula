namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// Filtrar Transferencia entre almacenes.
    /// </summary>
    public class TransferFilter
    {
        /// <summary>
        /// Id Almacén Origen.
        /// </summary>
        public int Origin { get; set; }

        /// <summary>
        /// Id Almacén destino.
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// Año del Inventario.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Mes del Inventario.
        /// </summary>
        public string Month { get; set; }
    }
}
