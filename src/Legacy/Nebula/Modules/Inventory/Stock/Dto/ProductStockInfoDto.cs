namespace Nebula.Modules.Inventory.Stock.Dto;

public class ProductStockInfoDto
{
    /// <summary>
    /// Identificador Almacén.
    /// </summary>
    public string WarehouseId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Almacén.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Identificador Producto.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de Productos.
    /// </summary>
    public decimal Quantity { get; set; }
}
