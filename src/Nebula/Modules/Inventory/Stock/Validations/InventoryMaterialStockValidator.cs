using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Validations;

public interface IInventoryMaterialStockValidator
{
    Task<Material> ValidarMaterial(string id);
}

public class InventoryMaterialStockValidator : IInventoryMaterialStockValidator
{
    private readonly IProductStockService _productStockService;
    private readonly IMaterialService _materialService;
    private readonly IMaterialDetailService _materialDetailService;

    public InventoryMaterialStockValidator(IProductStockService productStockService,
        IMaterialService materialService,
        IMaterialDetailService materialDetailService)
    {
        _productStockService = productStockService;
        _materialService = materialService;
        _materialDetailService = materialDetailService;
    }

    public async Task<Material> ValidarMaterial(string id)
    {
        var material = await _materialService.GetByIdAsync(id);
        var materialDetails = await _materialDetailService.GetListAsync(material.Id);
        var productStocks = new List<ProductStock>();
        materialDetails.ForEach(item =>
        {
            productStocks.Add(new ProductStock()
            {
                Id = string.Empty,
                WarehouseId = item.WarehouseId,
                ProductId = item.ProductId,
                Type = InventoryType.SALIDA,
                Quantity = item.CantUsado,
            });
        });
        await _productStockService.CreateManyAsync(productStocks);
        material.Status = InventoryStatus.VALIDADO;
        await _materialService.UpdateAsync(material.Id, material);
        return material;
    }
}
