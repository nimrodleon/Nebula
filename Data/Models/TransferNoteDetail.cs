using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    public class TransferNoteDetail
    {
        public int Id { get; set; }
        public int? TransferNoteId { get; set; }

        [JsonIgnore]
        [ForeignKey("TransferNoteId")]
        public TransferNote TransferNote { get; set; }

        public int? ProductId { get; set; }
        [MaxLength(250)] public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
    }
}
