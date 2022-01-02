using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Nebula.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        [MaxLength(250)]
        public string Description { get; set; }

        /// <summary>
        /// Código de Barra.
        /// </summary>
        [MaxLength(250)]
        public string Barcode { get; set; }

        /// <summary>
        /// Precio de producto.
        /// </summary>
        public decimal? Price1 { get; set; }

        /// <summary>
        /// Precio mayor del producto.
        /// </summary>
        public decimal? Price2 { get; set; }

        /// <summary>
        /// Cantidad minima para aplicar precio a mayor.
        /// </summary>
        public decimal? FromQty { get; set; }

        /// <summary>
        /// Tipo IGV Sunat.
        /// </summary>
        [MaxLength(250)]
        public string IgvSunat { get; set; }

        /// <summary>
        /// Impuesto a la bolsa plástica.
        /// </summary>
        [MaxLength(250)]
        public string Icbper { get; set; }

        /// <summary>
        /// Id Categoría.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Propiedad de Relación, Categoría.
        /// </summary>
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        /// <summary>
        /// Id unidad de medida.
        /// </summary>
        public int? UndMedidaId { get; set; }

        /// <summary>
        /// Propiedad de Relación, Unidad de Medida.
        /// </summary>
        [ForeignKey("UndMedidaId")]
        public UndMedida UndMedida { get; set; }

        /// <summary>
        /// Tipo de Bien/Servicio.
        /// </summary>
        [MaxLength(250)]
        public string Type { get; set; }

        /// <summary>
        /// Path de la imagen del producto.
        /// </summary>
        [MaxLength(250)]
        public string PathImage { get; set; }

        /// <summary>
        /// Propiedad para subir imágenes.
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
