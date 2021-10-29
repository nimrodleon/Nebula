using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    /// <summary>
    /// Detalle de caja diaria.
    /// </summary>
    public class CashierDetail
    {
        public int Id { get; set; }
        public int? CajaDiariaId { get; set; }
        [ForeignKey("CajaDiariaId")] public CajaDiaria CajaDiaria { get; set; }
        [DataType((DataType.Date))] public DateTime StartDate { get; set; }
        [MaxLength(250)] public string Document { get; set; }
        [MaxLength(250)] public string Contact { get; set; }
        [MaxLength(250)] public string Glosa { get; set; }
        [MaxLength(250)] public string PaymentType { get; set; }
        public decimal? Total { get; set; }
    }
}
