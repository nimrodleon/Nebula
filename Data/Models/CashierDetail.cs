using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public enum TypeOperation
    {
        CajaChica,
        Comprobante
    }

    /// <summary>
    /// Detalle de caja diaria.
    /// </summary>
    public class CashierDetail
    {
        public int Id { get; set; }
        public int? CajaDiariaId { get; set; }
        [ForeignKey("CajaDiariaId")] public CajaDiaria CajaDiaria { get; set; }
        public int? InvoiceId { get; set; }
        [ForeignKey("InvoiceId")] public Invoice Invoice { get; set; }
        public TypeOperation? TypeOperation { get; set; }
        [DataType((DataType.Date))] public DateTime StartDate { get; set; }
        [MaxLength(250)] public string Document { get; set; }
        [MaxLength(250)] public string Contact { get; set; }
        [MaxLength(250)] public string Glosa { get; set; }
        [MaxLength(250)] public string Type { get; set; }
        public decimal? Total { get; set; }
    }
}
