using System.ComponentModel.DataAnnotations;

namespace Nebula.Data.Models
{
    public class Company
    {
        public int Id { get; set; }
        [MaxLength(250)] public string Ruc { get; set; }
        [MaxLength(250)] public string RznSocial { get; set; }
        [MaxLength(250)] public string CodLocalEmisor { get; set; }
        [MaxLength(250)] public string TipMoneda { get; set; }
        public decimal? PorcentajeIgv { get; set; }
        public decimal? ValorImpuestoBolsa { get; set; }
        public decimal? CompletarDatosBoleta { get; set; }
        [MaxLength(250)] public string CuentaBancoDetraccion { get; set; }
        [MaxLength(250)] public string TextoDetraccion { get; set; }
        public decimal? MontoDetraccion { get; set; }
        public int? ContactId { get; set; }
    }
}
