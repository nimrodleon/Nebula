using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public enum TypeOperation
    {
        CajaChica,
        Comprobante
    }

    /// <summary>
    /// Detalle de caja diaria.
    /// </summary>
    public class CashierDetail
    {
        public int Id { get; set; }

        /// <summary>
        /// Clave foránea CajaDiaria.
        /// </summary>
        public int? CajaDiariaId { get; set; }

        /// <summary>
        /// Propiedad de Relación con Caja diaria.
        /// </summary>
        [ForeignKey("CajaDiariaId")]
        public CajaDiaria CajaDiaria { get; set; }

        /// <summary>
        /// Clave foránea Factura.
        /// </summary>
        public int? InvoiceId { get; set; }

        /// <summary>
        /// Propiedad de Relación con Factura.
        /// </summary>
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Tipo de Operación.
        /// </summary>
        public TypeOperation? TypeOperation { get; set; }

        /// <summary>
        /// Fecha de registro.
        /// </summary>
        [DataType((DataType.Date))]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Serie y Número de documento.
        /// </summary>
        [MaxLength(250)]
        public string Document { get; set; }

        /// <summary>
        /// Nombre Contacto.
        /// </summary>
        [MaxLength(250)]
        public string Contact { get; set; }

        /// <summary>
        /// Observación de la Operación.
        /// </summary>
        [MaxLength(250)]
        public string Glosa { get; set; }

        /// <summary>
        /// Medios de Pago.
        /// </summary>
        public int? PaymentMethod { get; set; }

        /// <summary>
        /// Movimiento de efectivo,
        /// (Entrada|Salida).
        /// </summary>
        [MaxLength(250)]
        public string Type { get; set; }

        /// <summary>
        /// Monto de la Operación.
        /// </summary>
        public decimal? Total { get; set; }
    }
}
