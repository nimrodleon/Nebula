using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class AjusteInventarioToProductStockConverter
{
    public List<ProductStock> Convertir(AjusteInventarioDto dto)
    {
        var productStocks = new List<ProductStock>();
        dto.AjusteInventarioDetails.ForEach(item =>
        {
            productStocks.Add(new ProductStock()
            {
                Id = string.Empty,
                CompanyId = dto.AjusteInventario.CompanyId.Trim(),
                WarehouseId = dto.AjusteInventario.WarehouseId,
                ProductId = item.ProductId,
                TransactionType = TransactionType.ENTRADA,
                Quantity = item.CantContada,
            });
        });
        return productStocks;
    }
}
