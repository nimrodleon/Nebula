namespace Nebula.Database.ViewModels.Inventory;

public class ProductStockReport
{
    public string WarehouseId { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public long Quantity { get; set; }
}
