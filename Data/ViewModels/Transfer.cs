using System;
using System.Collections.Generic;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// Nota Transferencia entre almacenes.
    /// </summary>
    public class Transfer
    {
        public Guid Origin { get; set; }
        public Guid Target { get; set; }
        public int Motivo { get; set; }
        public DateTime StartDate { get; set; }
        public string Remark { get; set; }
        public List<ItemNote> ItemNotes { get; set; }
    }
}
