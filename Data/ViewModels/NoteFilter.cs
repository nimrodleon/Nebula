namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// Filtrar Notas de Inventario.
    /// </summary>
    public class NoteFilter
    {
        /// <summary>
        /// Tipo de Nota (Ingreso|Salida).
        /// </summary>
        public string NoteType { get; set; }

        /// <summary>
        /// Id del Almacén.
        /// </summary>
        public string WarehouseId { get; set; }

        /// <summary>
        /// Año.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Mes.
        /// </summary>
        public string Month { get; set; }
    }
}
