using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    /// <summary>
    /// Motivos para tipos de inventario.
    /// </summary>
    public class InventoryReason
    {
        public int Id { get; set; }
        [MaxLength(250)] public string Description { get; set; }

        /// <summary>
        /// Transfer|Input|Output
        /// </summary>
        [MaxLength(250)]
        public string Type { get; set; }
    }
}
