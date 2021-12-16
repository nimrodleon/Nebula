using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }

        /// <summary>
        /// Documento de Identidad.
        /// </summary>
        [MaxLength(250)]
        public string Document { get; set; }

        /// <summary>
        /// Tipo de documento.
        /// </summary>
        public int? DocType { get; set; }

        /// <summary>
        /// Nombre de contacto.
        /// </summary>
        [MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Dirección de contacto.
        /// </summary>
        [MaxLength(250)]
        public string Address { get; set; }

        /// <summary>
        /// Número Telefónico.
        /// </summary>
        [MaxLength(250)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// E-Mail de Contacto.
        /// </summary>
        [MaxLength(250)]
        public string Email { get; set; }
    }
}
