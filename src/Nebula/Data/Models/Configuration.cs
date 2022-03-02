namespace Nebula.Data.Models
{
    public class Configuration
    {
        public string Id { get; set; }

        /// <summary>
        /// R.U.C. Empresa.
        /// </summary>
        public string Ruc { get; set; }

        /// <summary>
        /// Razón Social.
        /// </summary>
        public string RznSocial { get; set; }

        /// <summary>
        /// Código Local Emisor.
        /// </summary>
        public string CodLocalEmisor { get; set; }

        /// <summary>
        /// Tipo de moneda.
        /// </summary>
        public string TipMoneda { get; set; }

        /// <summary>
        /// Porcentaje IGV.
        /// </summary>
        public decimal PorcentajeIgv { get; set; }

        /// <summary>
        /// Monto Impuesto a la Bolsa plástica.
        /// </summary>
        public decimal ValorImpuestoBolsa { get; set; }

        /// <summary>
        /// Tipo de Emisión Electrónica.
        /// </summary>
        public string CpeSunat { get; set; }

        /// <summary>
        /// Contacto por defecto para operaciones
        /// menores a 700 soles con boleta.
        /// </summary>
        public string ContactId { get; set; }

        /// <summary>
        /// URL Api. CPE - SUNAT.
        /// </summary>
        public string UrlApi { get; set; }

        /// <summary>
        /// Path Archivos SUNAT.
        /// </summary>
        public string FileSunat { get; set; }

        /// <summary>
        /// Path Archivos de Control.
        /// </summary>
        public string FileControl { get; set; }
    }
}
