using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Notas;

namespace Nebula.Modules.Inventory.Stock.Validations;

public interface IInventoryNotasStockValidator
{
    Task<InventoryNotas> ValidarNotas(string companyId, string id);
}

public class InventoryNotasStockValidator : IInventoryNotasStockValidator
{
    private readonly IProductStockService _productStockService;
    private readonly IInventoryNotasService _inventoryNotasService;
    private readonly IInventoryNotasDetailService _inventoryNotasDetailService;

    public InventoryNotasStockValidator(IProductStockService productStockService,
        IInventoryNotasService inventoryNotasService,
        IInventoryNotasDetailService inventoryNotasDetailService)
    {
        _productStockService = productStockService;
        _inventoryNotasService = inventoryNotasService;
        _inventoryNotasDetailService = inventoryNotasDetailService;
    }

    public async Task<InventoryNotas> ValidarNotas(string companyId, string id)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(companyId, id);
        var inventoryNotasDetails = await _inventoryNotasDetailService.GetListAsync(companyId, inventoryNotas.Id);
        var productStocks = new List<ProductStock>();
        inventoryNotasDetails.ForEach(item =>
        {
            productStocks.Add(new ProductStock
            {
                Id = string.Empty,
                CompanyId = companyId.Trim(),
                WarehouseId = inventoryNotas.WarehouseId,
                ProductId = item.ProductId,
                Type = inventoryNotas.Type,
                Quantity = item.Demanda
            });
        });
        await _productStockService.CreateManyAsync(productStocks);
        inventoryNotas.Status = InventoryStatus.VALIDADO;
        await _inventoryNotasService.UpdateAsync(inventoryNotas.Id, inventoryNotas);
        return inventoryNotas;
    }
}
