using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class InventoryNoteToProductStockConverter
{
    public List<ProductStock> Convertir(InventoryNoteDto notaDto)
    {
        var productStocks = new List<ProductStock>();
        notaDto.InventoryNotasDetail.ForEach(item =>
        {
            productStocks.Add(new ProductStock
            {
                Id = string.Empty,
                CompanyId = notaDto.InventoryNotas.CompanyId,
                WarehouseId = notaDto.InventoryNotas.WarehouseId,
                ProductId = item.ProductId,
                TransactionType = notaDto.InventoryNotas.TransactionType,
                Quantity = item.Demanda
            });
        });
        return productStocks;
    }
}
