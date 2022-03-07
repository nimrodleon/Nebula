using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    // TODO: refactoring.
    public class TypeOperationSunat
    {
        /// <summary>
        /// Clave Primaria.
        /// </summary>
        [Key]
        [MaxLength(250)]
        public string Id { get; set; }

        /// <summary>
        /// Descripción del Tipo de Operación.
        /// </summary>
        [MaxLength(250)]
        public string Description { get; set; }
    }
}
