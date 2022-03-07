using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    // TODO: refactoring.
    public class TransferNoteDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Id Nota de Transferencia.
        /// </summary>
        public int? TransferNoteId { get; set; }

        /// <summary>
        /// Propiedad de Relación, Nota de Trasferencia.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("TransferNoteId")]
        public TransferNote TransferNote { get; set; }

        /// <summary>
        /// Id del producto.
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        [MaxLength(250)]
        public string Description { get; set; }

        /// <summary>
        /// Precio del producto.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Cantidad del producto.
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// Monto del Item (cantidad * precio).
        /// </summary>
        public decimal? Amount { get; set; }
    }
}
