using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class InventoryNoteToProductStockConverter
{
    private readonly InventoryNoteDto _inventoryNoteDto;

    public InventoryNoteToProductStockConverter(InventoryNoteDto inventoryNoteDto)
    {
        _inventoryNoteDto = inventoryNoteDto;
    }

    public List<ProductStock> Convertir()
    {
        var productStocks = new List<ProductStock>();
        _inventoryNoteDto.InventoryNotasDetail.ForEach(item =>
        {
            productStocks.Add(new ProductStock
            {
                Id = string.Empty,
                CompanyId = _inventoryNoteDto.InventoryNotas.CompanyId,
                WarehouseId = _inventoryNoteDto.InventoryNotas.WarehouseId,
                ProductId = item.ProductId,
                TransactionType = _inventoryNoteDto.InventoryNotas.TransactionType,
                Quantity = item.Demanda
            });
        });
        return productStocks;
    }
}
