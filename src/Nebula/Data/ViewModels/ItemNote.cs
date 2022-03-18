namespace Nebula.Data.ViewModels;

/// <summary>
/// Detalle Nota de Inventario.
/// </summary>
public class ItemNote
{
    /// <summary>
    /// Id del producto.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Descripción del Item o nombre del producto.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Cantidad del Item.
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Precio del Producto/Servicio.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Monto del Item (cantidad * precio).
    /// </summary>
    public decimal Amount { get; set; }
}
