namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para la emisión de comprobantes.
    /// </summary>
    public class DetalleComprobante
    {
        /// <summary>
        /// Numero del Item.
        /// </summary>
        public int NumItem { get; set; }

        /// <summary>
        /// Id del producto.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Descripción Item o Nombre del producto.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Precio del Item.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Cantidad del Producto/Servicio.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Monto del Item (cantidad * precio).
        /// </summary>
        public decimal Amount { get; set; }
    }
}
