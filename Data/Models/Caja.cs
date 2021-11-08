using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nebula.Data.Models
{
    public class Caja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(250)] public string Name { get; set; }

        [JsonIgnore] public List<CajaDiaria> CajasDiaria { get; set; }
        [JsonIgnore] public List<SerieInvoice> SerieInvoices { get; set; }
    }
}
