using System;
using System.Collections.Generic;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// Nota Transferencia entre almacenes.
    /// </summary>
    public class Transfer
    {
        public string Origin { get; set; }
        public string Target { get; set; }
        public string Motivo { get; set; }
        public DateTime StartDate { get; set; }
        public string Remark { get; set; }
        public List<ItemNote> ItemNotes { get; set; }
    }
}
