using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class InventoryNote
    {
        public int Id { get; set; }
        public int? ContactId { get; set; }
        public string WarehouseId { get; set; }
        [MaxLength(250)] public string Motivo { get; set; }
        [DataType(DataType.Date)] public DateTime StartDate { get; set; }
        [MaxLength(250)] public string Remark { get; set; }
        [MaxLength(250)] public string Status { get; set; }
        public List<InventoryNoteDetail> InventoryNoteDetails { get; set; }
    }
}
