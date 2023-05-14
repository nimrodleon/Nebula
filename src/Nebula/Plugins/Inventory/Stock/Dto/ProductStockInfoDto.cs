namespace Nebula.Plugins.Inventory.Stock.Dto;

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
    /// Identificador del Lote del producto.
    /// </summary>
    public string ProductLoteId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Lote del producto.
    /// </summary>
    public string ProductLoteName { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de Productos.
    /// </summary>
    public long Quantity { get; set; }

    /// <summary>
    /// Fecha de Vencimiento.
    /// </summary>
    public string ExpirationDate { get; set; } = string.Empty;
}
