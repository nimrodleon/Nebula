namespace Nebula.Modules.Inventory.Dto;

public class LocationItemStockDto
{
    public string ProductId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public long QuantityMax { get; set; }
    public long QuantityMin { get; set; }
    public decimal Stock { get; set; }
}
