using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class MaterialToProductStockConverter
{
    private readonly MaterialDto _materialDto;

    public MaterialToProductStockConverter(MaterialDto materialDto)
    {
        _materialDto = materialDto;
    }

    public List<ProductStock> Convertir()
    {
        var productStocks = new List<ProductStock>();
        _materialDto.MaterialDetails.ForEach(item =>
         {
             productStocks.Add(new ProductStock()
             {
                 Id = string.Empty,
                 CompanyId = _materialDto.Material.CompanyId.Trim(),
                 WarehouseId = item.WarehouseId,
                 ProductId = item.ProductId,
                 TransactionType = TransactionType.SALIDA,
                 Quantity = item.CantUsado,
             });
         });
        return productStocks;
    }
}
