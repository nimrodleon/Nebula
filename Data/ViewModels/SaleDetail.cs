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
    }
}
