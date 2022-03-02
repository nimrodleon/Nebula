namespace Nebula.Data.Models
{
    public class Contact
    {
        public string Id { get; set; }

        /// <summary>
        /// Documento de Identidad.
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Tipo de documento.
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// Nombre de contacto.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dirección de contacto.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Número Telefónico.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// E-Mail de Contacto.
        /// </summary>
        public string Email { get; set; }
    }
}
