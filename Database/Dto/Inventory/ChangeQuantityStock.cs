namespace Nebula.Database.Dto.Inventory;

public class ChangeQuantityStock
{
    public string WarehouseId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public long Quantity { get; set; }
}
