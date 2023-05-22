using Nebula.Modules.Inventory.Models;

namespace Nebula.Database.Dto.Inventory;

public class RespLocationDetailStock
{
    public Location Location { get; set; } = new Location();
    public List<LocationItemStockDto> LocationDetailStocks { get; set; } = new List<LocationItemStockDto>();
}
