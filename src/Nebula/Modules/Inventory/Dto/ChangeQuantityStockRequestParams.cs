namespace Nebula.Modules.Inventory.Dto;

public class ChangeQuantityStockRequestParams
{
    public string WarehouseId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
}
