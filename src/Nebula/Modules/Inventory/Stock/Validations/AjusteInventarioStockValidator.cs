using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Validations;

public interface IAjusteInventarioStockValidator
{
    Task<AjusteInventario> ValidarAjusteInventario(string id);
}

public class AjusteInventarioStockValidator : IAjusteInventarioStockValidator
{
    private readonly IProductStockService _productStockService;
    private readonly IAjusteInventarioService _ajusteInventarioService;
    private readonly IAjusteInventarioDetailService _ajusteInventarioDetailService;

    public AjusteInventarioStockValidator(IProductStockService productStockService,
        IAjusteInventarioService ajusteInventarioService,
        IAjusteInventarioDetailService ajusteInventarioDetailService)
    {
        _productStockService = productStockService;
        _ajusteInventarioService = ajusteInventarioService;
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
    }

    public async Task<AjusteInventario> ValidarAjusteInventario(string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetByIdAsync(id);
        var ajusteInventarioDetails = await _ajusteInventarioDetailService.GetListAsync(ajusteInventario.Id);
        ajusteInventarioDetails = await _productStockService.CalcularCantidadExistenteAjusteInventarioAsync(ajusteInventarioDetails, ajusteInventario.WarehouseId);
        var productArrId = new List<string>();
        ajusteInventarioDetails.ForEach(item => productArrId.Add(item.ProductId));
        await _productStockService.DeleteProductStockByWarehouseIdAsync(ajusteInventario.WarehouseId, productArrId);
        var productStocks = new List<ProductStock>();
        ajusteInventarioDetails.ForEach(item =>
        {
            productStocks.Add(new ProductStock()
            {
                Id = string.Empty,
                WarehouseId = ajusteInventario.WarehouseId,
                ProductId = item.ProductId,
                Type = InventoryType.ENTRADA,
                Quantity = item.CantContada,
            });
        });
        await _productStockService.CreateManyAsync(productStocks);
        await _ajusteInventarioDetailService.DeleteManyAsync(ajusteInventario.Id);
        await _ajusteInventarioDetailService.InsertManyAsync(ajusteInventarioDetails);
        ajusteInventario.Status = InventoryStatus.VALIDADO;
        await _ajusteInventarioService.UpdateAsync(ajusteInventario.Id, ajusteInventario);
        return ajusteInventario;
    }
}
