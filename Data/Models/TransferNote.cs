using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class TransferNote
    {
        public int Id { get; set; }
        public Guid? OriginId { get; set; }

        /// <summary>
        /// Almacén Origen.
        /// </summary>
        [ForeignKey("OriginId")]
        public Warehouse Origin { get; set; }

        public Guid? TargetId { get; set; }

        /// <summary>
        /// Almacén Destino.
        /// </summary>
        [ForeignKey("TargetId")]
        public Warehouse Target { get; set; }

        [MaxLength(250)] public string Motivo { get; set; }
        [DataType(DataType.Date)] public DateTime StartDate { get; set; }
        [MaxLength(250)] public string Remark { get; set; }
        [MaxLength(250)] public string Status { get; set; }
        [MaxLength(250)] public string Year { get; set; }
        [MaxLength(250)] public string Month { get; set; }
        public List<TransferNoteDetail> TransferNoteDetails { get; set; }
    }
}
