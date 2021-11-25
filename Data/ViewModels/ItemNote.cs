namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// Detalle Nota de Inventario.
    /// </summary>
    public class ItemNote
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
