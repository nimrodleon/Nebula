using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class CajaDiaria
    {
        public int Id { get; set; }
        public Guid? CajaId { get; set; }
        [ForeignKey("CajaId")] public Caja Caja { get; set; }
        [MaxLength(250)] public string Name { get; set; }
        [DataType(DataType.Date)] public DateTime StartDate { get; set; }
        [MaxLength(250)] public string State { get; set; }
        public decimal TotalApertura { get; set; }
        public decimal TotalContabilizado { get; set; }
        public decimal TotalCierre { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
    }
}
