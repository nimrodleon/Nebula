using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class MaterialToProductStockConverter
{
    public List<ProductStock> Convertir(MaterialDto dto)
    {
        var productStocks = new List<ProductStock>();
        dto.MaterialDetails.ForEach(item =>
        {
            productStocks.Add(new ProductStock()
            {
                Id = string.Empty,
                CompanyId = dto.Material.CompanyId.Trim(),
                WarehouseId = item.WarehouseId,
                ProductId = item.ProductId,
                TransactionType = TransactionType.SALIDA,
                Quantity = item.CantUsado,
            });
        });
        return productStocks;
    }
}
