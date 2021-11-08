using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    public class SerieInvoice
    {
        public int Id { get; set; }
        [MaxLength(250)] public string Prefix { get; set; }
        public int? Counter { get; set; }
        [MaxLength(250)] public string TypeDoc { get; set; }
        public Guid? CajaId { get; set; }
        [ForeignKey("CajaId")] public Caja Caja { get; set; }
    }
}
