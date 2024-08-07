using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Dto;

public class TransferenciaResult
{
    public List<ProductStock> ProductStockOrigen { get; set; } = new List<ProductStock>();
    public List<ProductStock> ProductStockDestino { get; set; } = new List<ProductStock>();
}
