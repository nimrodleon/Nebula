using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory
{
    public class ValidateStockService
    {
        private readonly ProductStockService _productStockService;
        private readonly InventoryNotasService _inventoryNotasService;
        private readonly InventoryNotasDetailService _inventoryNotasDetailService;
        private readonly TransferenciaService _transferenciaService;
        private readonly TransferenciaDetailService _transferenciaDetailService;

        public ValidateStockService(ProductStockService productStockService, InventoryNotasService inventoryNotasService, InventoryNotasDetailService inventoryNotasDetailService, TransferenciaService transferenciaService, TransferenciaDetailService transferenciaDetailService)
        {
            _productStockService = productStockService;
            _inventoryNotasService = inventoryNotasService;
            _inventoryNotasDetailService = inventoryNotasDetailService;
            _transferenciaService = transferenciaService;
            _transferenciaDetailService = transferenciaDetailService;
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

        public async Task<Transferencia> ValidarTransferencia(string id)
        {
            var transferencia = await _transferenciaService.GetAsync(id);
            var transferenciaDetails = await _transferenciaDetailService.GetListAsync(transferencia.Id);
            var productStocksEntrada = new List<ProductStock>();
            var productStocksSalida = new List<ProductStock>();
            transferenciaDetails.ForEach(item =>
            {
                productStocksEntrada.Add(new ProductStock()
                {
                    Id = string.Empty,
                    WarehouseId = transferencia.WarehouseTargetId,
                    ProductId = item.ProductId,
                    Type = InventoryType.ENTRADA,
                    Quantity = item.CantTransferido,
                });
                productStocksSalida.Add(new ProductStock()
                {
                    Id = string.Empty,
                    WarehouseId = transferencia.WarehouseOriginId,
                    ProductId = item.ProductId,
                    Type = InventoryType.SALIDA,
                    Quantity = item.CantTransferido,
                });
            });
            await _productStockService.CreateAsync(productStocksEntrada);
            await _productStockService.CreateAsync(productStocksSalida);
            transferenciaDetails = await _productStockService.GetTransferenciaDetailsAsync(transferenciaDetails, transferencia.WarehouseOriginId);
            await _transferenciaDetailService.DeleteManyAsync(transferencia.Id);
            await _transferenciaDetailService.InsertManyAsync(transferenciaDetails);
            transferencia.Status = InventoryStatus.VALIDADO;
            await _transferenciaService.UpdateAsync(transferencia.Id, transferencia);
            return transferencia;
        }
    }
}
