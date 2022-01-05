using System;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// Cuota de Créditos.
    /// </summary>
    public class Cuota
    {
        public int? Id { get; set; }

        /// <summary>
        /// Número de cuota.
        /// </summary>
        public int NumCuota { get; set; }

        /// <summary>
        /// Fecha de vencimiento.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Monto prometido.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
