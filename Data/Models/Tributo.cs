using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    public class Tributo
    {
        public int Id { get; set; }

        /// <summary>
        /// foreignKey in db.
        /// </summary>
        public int? InvoiceId { get; set; }

        /// <summary>
        /// propiedad de relación.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Identificador de tributo.
        /// </summary>
        [MaxLength(250)]
        public string IdeTributo { get; set; }

        /// <summary>
        /// Nombre de tributo.
        /// </summary>
        [MaxLength(250)]
        public string NomTributo { get; set; }

        /// <summary>
        /// Código de tipo de tributo.
        /// </summary>
        [MaxLength(250)]
        public string CodTipTributo { get; set; }

        /// <summary>
        /// Base imponible.
        /// </summary>
        public decimal? MtoBaseImponible { get; set; }

        /// <summary>
        /// Monto de Tributo.
        /// </summary>
        public decimal? MtoTributo { get; set; }

        /// <summary>
        /// Año de registro.
        /// </summary>
        [MaxLength(250)]
        public string Year { get; set; }

        /// <summary>
        /// Mes de registro.
        /// </summary>
        [MaxLength(250)]
        public string Month { get; set; }
    }
}
