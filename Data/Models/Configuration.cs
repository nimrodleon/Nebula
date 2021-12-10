using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class Configuration
    {
        public int Id { get; set; }

        /// <summary>
        /// R.U.C. Empresa.
        /// </summary>
        [MaxLength(250)]
        public string Ruc { get; set; }

        /// <summary>
        /// Razón Social.
        /// </summary>
        [MaxLength(250)]
        public string RznSocial { get; set; }

        /// <summary>
        /// Código Local Emisor.
        /// </summary>
        [MaxLength(250)]
        public string CodLocalEmisor { get; set; }

        /// <summary>
        /// Tipo de moneda.
        /// </summary>
        [MaxLength(250)]
        public string TipMoneda { get; set; }

        /// <summary>
        /// Porcentaje IGV.
        /// </summary>
        public decimal? PorcentajeIgv { get; set; }

        /// <summary>
        /// Monto Impuesto a la Bolsa plástica.
        /// </summary>
        public decimal? ValorImpuestoBolsa { get; set; }

        /// <summary>
        /// Monto para completar datos del cliente en la boleta.
        /// </summary>
        public decimal? CompletarDatosBoleta { get; set; }

        /// <summary>
        /// Cuenta de detracción.
        /// </summary>
        [MaxLength(250)]
        public string CuentaBancoDetraccion { get; set; }

        /// <summary>
        /// Texto para mostrar en facturas.
        /// </summary>
        [MaxLength(250)]
        public string TextoDetraccion { get; set; }

        /// <summary>
        /// Monto mínimo para la detracción.
        /// </summary>
        public decimal? MontoDetraccion { get; set; }

        /// <summary>
        /// Contacto por defecto para operaciones
        /// menores a 700 soles con boleta.
        /// </summary>
        public int? ContactId { get; set; }
    }
}
