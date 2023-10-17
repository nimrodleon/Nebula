using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class AjusteInventarioToProductStockConverter
{
    private readonly AjusteInventarioDto _ajusteInventarioDto;

    public AjusteInventarioToProductStockConverter(AjusteInventarioDto ajusteInventarioDto)
    {
        _ajusteInventarioDto = ajusteInventarioDto;
    }

    public List<ProductStock> Convertir()
    {
        var productStocks = new List<ProductStock>();
        _ajusteInventarioDto.AjusteInventarioDetails.ForEach(item =>
        {
            productStocks.Add(new ProductStock()
            {
                Id = string.Empty,
                CompanyId = _ajusteInventarioDto.AjusteInventario.CompanyId.Trim(),
                WarehouseId = _ajusteInventarioDto.AjusteInventario.WarehouseId,
                ProductId = item.ProductId,
                TransactionType = TransactionType.ENTRADA,
                Quantity = item.CantContada,
            });
        });
        return productStocks;
    }
}
