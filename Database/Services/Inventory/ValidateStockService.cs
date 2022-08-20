using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory;

public class ValidateStockService
{
    private readonly ProductStockService _productStockService;
    private readonly InventoryNotasService _inventoryNotasService;
    private readonly InventoryNotasDetailService _inventoryNotasDetailService;
    private readonly TransferenciaService _transferenciaService;
    private readonly TransferenciaDetailService _transferenciaDetailService;
    private readonly AjusteInventarioService _ajusteInventarioService;
    private readonly AjusteInventarioDetailService _ajusteInventarioDetailService;
    private readonly MaterialService _materialService;
    private readonly MaterialDetailService _materialDetailService;

    public ValidateStockService(ProductStockService productStockService, InventoryNotasService inventoryNotasService, InventoryNotasDetailService inventoryNotasDetailService, TransferenciaService transferenciaService, TransferenciaDetailService transferenciaDetailService, AjusteInventarioService ajusteInventarioService, AjusteInventarioDetailService ajusteInventarioDetailService, MaterialService materialService, MaterialDetailService materialDetailService)
    {
        _productStockService = productStockService;
        _inventoryNotasService = inventoryNotasService;
        _inventoryNotasDetailService = inventoryNotasDetailService;
        _transferenciaService = transferenciaService;
        _transferenciaDetailService = transferenciaDetailService;
        _ajusteInventarioService = ajusteInventarioService;
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
        _materialService = materialService;
        _materialDetailService = materialDetailService;
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

    public async Task<AjusteInventario> ValidarAjusteInventario(string id)
    {
        var ajusteInventario = await _ajusteInventarioService.GetAsync(id);
        var ajusteInventarioDetails = await _ajusteInventarioDetailService.GetListAsync(ajusteInventario.Id);
        ajusteInventarioDetails = await _productStockService.GetAjusteInventarioDetailsAsync(ajusteInventarioDetails, ajusteInventario.WarehouseId);
        await _productStockService.ClearAjusteInventarioDetailAsync(ajusteInventarioDetails, ajusteInventario.WarehouseId);
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
        await _productStockService.CreateAsync(productStocks);
        await _ajusteInventarioDetailService.DeleteManyAsync(ajusteInventario.Id);
        await _ajusteInventarioDetailService.InsertManyAsync(ajusteInventarioDetails);
        ajusteInventario.Status = InventoryStatus.VALIDADO;
        await _ajusteInventarioService.UpdateAsync(ajusteInventario.Id, ajusteInventario);
        return ajusteInventario;
    }

    public async Task<Material> ValidarMaterial(string id)
    {
        var material = await _materialService.GetAsync(id);
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
        await _productStockService.CreateAsync(productStocks);
        material.Status = InventoryStatus.VALIDADO;
        await _materialService.UpdateAsync(material.Id, material);
        return material;
    }
}
