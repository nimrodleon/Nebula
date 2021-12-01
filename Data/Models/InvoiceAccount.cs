using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    public class InvoiceAccount
    {
        public int Id { get; set; }

        /// <summary>
        /// foreignKey in db.
        /// </summary>
        public int? InvoiceId { get; set; }

        /// <summary>
        /// propiedad de relación.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Serie comprobante.
        /// </summary>
        [MaxLength(250)]
        public string Serie { get; set; }

        /// <summary>
        /// Número comprobante.
        /// </summary>
        [MaxLength(250)]
        public string Number { get; set; }

        /// <summary>
        /// Tipo factura (Cobrar|Pagar).
        /// </summary>
        [MaxLength(250)]
        public string AccountType { get; set; }

        /// <summary>
        /// Estado Cuenta (PENDIENTE|COBRADO|ANULADO).
        /// </summary>
        [MaxLength(250)]
        public string Status { get; set; }

        /// <summary>
        /// Número de Cuota.
        /// </summary>
        public int? Cuota { get; set; }

        /// <summary>
        /// Monto Cuenta.
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// Saldo de la Cuenta.
        /// </summary>
        public decimal? Balance { get; set; }

        /// <summary>
        /// Fecha Vencimiento.
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// Año de registro.
        /// </summary>
        [MaxLength(250)]
        public string Year { get; set; }

        /// <summary>
        /// Mes de registro.
        /// </summary>
        [MaxLength(250)]
        public string Month { get; set; }
    }
}
