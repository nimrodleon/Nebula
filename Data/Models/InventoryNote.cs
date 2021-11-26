using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class InventoryNote
    {
        public int Id { get; set; }
        public int? ContactId { get; set; }

        /// <summary>
        /// Clave foránea Contacto.
        /// </summary>
        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }

        public Guid? WarehouseId { get; set; }

        /// <summary>
        /// Clave foránea Almacén.
        /// </summary>
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        [MaxLength(250)] public string NoteType { get; set; }
        [MaxLength(250)] public string Motivo { get; set; }
        [DataType(DataType.Date)] public DateTime StartDate { get; set; }
        [MaxLength(250)] public string Remark { get; set; }
        [MaxLength(250)] public string Status { get; set; }
        [MaxLength(250)] public string Year { get; set; }
        [MaxLength(250)] public string Month { get; set; }
        public List<InventoryNoteDetail> InventoryNoteDetails { get; set; }
    }
}
