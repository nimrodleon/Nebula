using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Nebula.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(250)] public string Description { get; set; }
        [MaxLength(250)] public string Barcode { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? FromQty { get; set; }
        [MaxLength(250)] public string IgvSunat { get; set; }
        [MaxLength(250)] public string Icbper { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")] public Category Category { get; set; }

        public Guid? UndMedidaId { get; set; }
        [ForeignKey("UndMedidaId")] public UndMedida UndMedida { get; set; }

        [MaxLength(250)] public string Type { get; set; }
        [MaxLength(250)] public string PathImage { get; set; }
        [JsonIgnore] [NotMapped] public IFormFile File { get; set; }
    }
}
