namespace Nebula.Data.ViewModels
{
    public class CerrarCaja
    {
        /// <summary>
        /// Monto contabilizado en caja.
        /// </summary>
        public decimal TotalContabilizado { get; set; }

        /// <summary>
        /// Monto para el día siguiente.
        /// </summary>
        public decimal TotalCierre { get; set; }
    }
}
