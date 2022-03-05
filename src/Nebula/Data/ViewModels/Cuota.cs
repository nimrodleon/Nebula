﻿namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// Cuota de Créditos.
    /// </summary>
    public class Cuota
    {
        public string Id { get; set; }

        /// <summary>
        /// Número de cuota.
        /// </summary>
        public int NumCuota { get; set; }

        /// <summary>
        /// Fecha de vencimiento.
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Monto prometido.
        /// </summary>
        public decimal Amount { get; set; }
    }
}