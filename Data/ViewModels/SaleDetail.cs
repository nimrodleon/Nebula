namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para el punto de venta.
    /// </summary>
    public class SaleDetail
    {
        /// <summary>
        /// Id del producto.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Descripción del Item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Precio del Item.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Cantidad del Producto.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Tipo de operación IGV Sunat.
        /// </summary>
        public string IgvSunat { get; set; }

        /// <summary>
        /// Monto IGV por Item.
        /// </summary>
        public decimal MtoIgvItem { get; set; }

        /// <summary>
        /// Base Imponible IGV por Item.
        /// </summary>
        public decimal MtoBaseIgvItem { get; set; }

        /// <summary>
        /// Precio Venta Item.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Tributo ICBPER.
        /// </summary>
        public bool TriIcbper { get; set; } = false;

        /// <summary>
        /// Monto Tributo ICBPER por Item.
        /// </summary>
        public decimal MtoTriIcbperItem { get; set; }
    }
}
