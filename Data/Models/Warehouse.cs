using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        /// <summary>
        /// Nombre Almacén.
        /// </summary>
        [MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Observación.
        /// </summary>
        [MaxLength(250)]
        public string Remark { get; set; }
    }
}
