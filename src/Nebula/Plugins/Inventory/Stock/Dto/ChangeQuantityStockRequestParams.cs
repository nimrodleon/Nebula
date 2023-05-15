namespace Nebula.Plugins.Inventory.Stock.Dto;

public class ChangeQuantityStockRequestParams
{
    public string WarehouseId { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string ProductLoteId { get; set; } = string.Empty;
    public long Quantity { get; set; }
}
