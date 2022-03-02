namespace Nebula.Data.Models
{
    public class Product
    {
        public string Id { get; set; }

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Código de Barra.
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Precio de producto.
        /// </summary>
        public decimal Price1 { get; set; }

        /// <summary>
        /// Precio mayor del producto.
        /// </summary>
        public decimal Price2 { get; set; }

        /// <summary>
        /// Cantidad minima para aplicar precio a mayor.
        /// </summary>
        public decimal FromQty { get; set; }

        /// <summary>
        /// Tipo IGV Sunat.
        /// </summary>
        public string IgvSunat { get; set; }

        /// <summary>
        /// Impuesto a la bolsa plástica.
        /// </summary>
        public string Icbper { get; set; }

        /// <summary>
        /// Categoría de producto.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Unidad de Medida.
        /// </summary>
        public string UndMedida { get; set; }

        /// <summary>
        /// Tipo de Bien/Servicio.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Path de la imagen del producto.
        /// </summary>
        public string PathImage { get; set; }
    }
}
