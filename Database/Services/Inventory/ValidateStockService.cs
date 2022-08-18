using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory
{
    public class ValidateStockService
    {
        private readonly ProductStockService _productStockService;
        private readonly InventoryNotasService _inventoryNotasService;
        private readonly InventoryNotasDetailService _inventoryNotasDetailService;

        public ValidateStockService(ProductStockService productStockService, InventoryNotasService inventoryNotasService, InventoryNotasDetailService inventoryNotasDetailService)
        {
            _productStockService = productStockService;
            _inventoryNotasService = inventoryNotasService;
            _inventoryNotasDetailService = inventoryNotasDetailService;
        }

        public async Task<InventoryNotas> ValidarNotas(string id)
        {
            var inventoryNotas = await _inventoryNotasService.GetAsync(id);
            var inventoryNotasDetails = await _inventoryNotasDetailService.GetListAsync(inventoryNotas.Id);
            var productStocks = new List<ProductStock>();
            inventoryNotasDetails.ForEach(item =>
            {
                productStocks.Add(new ProductStock
                {
                    Id = string.Empty,
                    WarehouseId = inventoryNotas.WarehouseId,
                    ProductId = item.ProductId,
                    Type = inventoryNotas.Type,
                    Quantity = item.Demanda
                });
            });
            await _productStockService.CreateAsync(productStocks);
            inventoryNotas.Status = InventoryStatus.VALIDADO;
            await _inventoryNotasService.UpdateAsync(inventoryNotas.Id, inventoryNotas);
            return inventoryNotas;
        }
    }
}
