using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class UndMedida
    {
        public int Id { get; set; }

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
