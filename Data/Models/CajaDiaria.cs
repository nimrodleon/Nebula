using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class CajaDiaria
    {
        public int Id { get; set; }

        /// <summary>
        /// Clave foránea Serie de facturación.
        /// </summary>
        public Guid? InvoiceSerieId { get; set; }

        /// <summary>
        /// Propiedad de relación con Series de facturación.
        /// </summary>
        [ForeignKey("InvoiceSerieId")]
        public InvoiceSerie InvoiceSerie { get; set; }

        /// <summary>
        /// Nombre identificador de Caja.
        /// </summary>
        [MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Fecha de Operación.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Estado Caja.
        /// </summary>
        [MaxLength(250)]
        public string Status { get; set; }

        /// <summary>
        /// Monto Apertura.
        /// </summary>
        public decimal TotalApertura { get; set; }

        /// <summary>
        /// Monto Contabilizado durante el dia.
        /// </summary>
        public decimal TotalContabilizado { get; set; }

        /// <summary>
        /// Monto para el dia siguiente.
        /// </summary>
        public decimal TotalCierre { get; set; }

        /// <summary>
        /// Año de registro.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Mes de registro.
        /// </summary>
        public string Month { get; set; }
    }
}
