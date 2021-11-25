using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class TransferNote
    {
        public int Id { get; set; }
        [MaxLength(250)] public string Origin { get; set; }
        [MaxLength(250)] public string Target { get; set; }
        [MaxLength(250)] public string Motivo { get; set; }
        [DataType(DataType.Date)] public DateTime StartDate { get; set; }
        [MaxLength(250)] public string Remark { get; set; }
        public List<TransferNoteDetail> TransferNoteDetails { get; set; }
    }
}
