using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.Data.Models
{
    /// <summary>
    /// Series de facturación.
    /// </summary>
    public class InvoiceSerie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador Serie.
        /// </summary>
        [MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Clave foránea Almacén.
        /// </summary>
        public Guid? WarehouseId { get; set; }

        /// <summary>
        /// Propiedad de relación con el modelo Almacén.
        /// </summary>
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; }

        /// <summary>
        /// Serie Factura.
        /// </summary>
        [MaxLength(250)]
        public string Factura { get; set; }

        /// <summary>
        /// Contador Factura.
        /// </summary>
        public int CounterFactura { get; set; }

        /// <summary>
        /// Serie Boleta.
        /// </summary>
        [MaxLength(250)]
        public string Boleta { get; set; }

        /// <summary>
        /// Contador Boleta.
        /// </summary>
        public int CounterBoleta { get; set; }

        /// <summary>
        /// Serie Nota de Venta.
        /// </summary>
        [MaxLength(250)]
        public string NotaDeVenta { get; set; }

        /// <summary>
        /// Contador Nota de Venta.
        /// </summary>
        public int CounterNotaDeVenta { get; set; }
    }
}
