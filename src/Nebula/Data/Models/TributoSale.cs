namespace Nebula.Data.Models
{
    public class TributoSale
    {
        public string Id { get; set; }

        /// <summary>
        /// foreignKey in db.
        /// </summary>
        public string InvoiceSale { get; set; }

        /// <summary>
        /// Identificador de tributo.
        /// </summary>
        public string IdeTributo { get; set; }

        /// <summary>
        /// Nombre de tributo.
        /// </summary>
        public string NomTributo { get; set; }

        /// <summary>
        /// Código de tipo de tributo.
        /// </summary>
        public string CodTipTributo { get; set; }

        /// <summary>
        /// Base imponible.
        /// </summary>
        public decimal MtoBaseImponible { get; set; }

        /// <summary>
        /// Monto de Tributo.
        /// </summary>
        public decimal MtoTributo { get; set; }

        /// <summary>
        /// Año de registro.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Mes de registro.
        /// </summary>
        public string Month { get; set; }
    }
}
