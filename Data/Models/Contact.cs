using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Id del Tipo de documento.
        /// </summary>
        public Guid? PeopleDocTypeId { get; set; }

        /// <summary>
        /// Propiedad de Relación, Tipo documento.
        /// </summary>
        [ForeignKey("PeopleDocTypeId")]
        public PeopleDocType PeopleDocType { get; set; }

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
