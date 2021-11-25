using System;
using System.Collections.Generic;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// Nota de Inventario.
    /// </summary>
    public class Note
    {
        public int ContactId { get; set; }
        public string WarehouseId { get; set; }
        public string Motivo { get; set; }
        public DateTime StartDate { get; set; }
        public string Remark { get; set; }
        public List<ItemNote> ItemNotes { get; set; }
    }
}
