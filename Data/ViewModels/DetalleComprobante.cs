namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para la emisión de comprobantes.
    /// </summary>
    public class DetalleComprobante
    {
        public int NumItem { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
