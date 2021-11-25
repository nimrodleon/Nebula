using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    public class InventoryNoteDetail
    {
        public int Id { get; set; }
        public int? InventoryNoteId { get; set; }

        [JsonIgnore]
        [ForeignKey("InventoryNoteId")]
        public InventoryNote InventoryNote { get; set; }

        public int? ProductId { get; set; }
        [MaxLength(250)] public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
    }
}
