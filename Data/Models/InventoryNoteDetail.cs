using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    public class InventoryNoteDetail
    {
        public int Id { get; set; }

        /// <summary>
        /// Id Nota de Inventario.
        /// </summary>
        public int? InventoryNoteId { get; set; }

        /// <summary>
        /// Propiedad de Relación, Nota de Inventario.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("InventoryNoteId")]
        public InventoryNote InventoryNote { get; set; }

        /// <summary>
        /// Id del Producto.
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// Descripción Item.
        /// </summary>
        [MaxLength(250)]
        public string Description { get; set; }

        /// <summary>
        /// Precio Item.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Cantidad del Item.
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// Monto del Item (cantidad * precio).
        /// </summary>
        public decimal? Amount { get; set; }
    }
}
