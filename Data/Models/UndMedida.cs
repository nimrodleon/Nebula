using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class UndMedida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre de la Unidad de medida.
        /// </summary>
        [MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Código Sunat.
        /// </summary>
        [MaxLength(250)]
        public string SunatCode { get; set; }
    }
}
