using Nebula.Database.Helpers;
using Nebula.Database.Models.Inventory;
using Nebula.Database.Services.Inventory;
using Nebula.Database.Services.Sales;
using Nebula.Plugins.Inventory.Models;

namespace Nebula.Plugins.Inventory.Stock;

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
    private readonly InvoiceSaleDetailService _invoiceSaleDetailService;

    public ValidateStockService(ProductStockService productStockService, InventoryNotasService inventoryNotasService, InventoryNotasDetailService inventoryNotasDetailService, TransferenciaService transferenciaService, TransferenciaDetailService transferenciaDetailService, AjusteInventarioService ajusteInventarioService, AjusteInventarioDetailService ajusteInventarioDetailService, MaterialService materialService, MaterialDetailService materialDetailService, InvoiceSaleDetailService invoiceSaleDetailService)
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
        _invoiceSaleDetailService = invoiceSaleDetailService;
    }

    public async Task<InventoryNotas> ValidarNotas(string id)
    {
        var inventoryNotas = await _inventoryNotasService.GetByIdAsync(id);
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
        var transferencia = await _transferenciaService.GetByIdAsync(id);
        var transferenciaDetails = await _transferenciaDetailService.GetListAsync(transferencia.Id);
        var productStockDestino = new List<ProductStock>();
        var productStockOrigen = new List<ProductStock>();
        transferenciaDetails.ForEach(item =>
        {
            productStockDestino.Add(new ProductStock()
            {
                Id = string.Empty,
                WarehouseId = transferencia.WarehouseTargetId,
                ProductId = item.ProductId,
                Type = InventoryType.ENTRADA,
                Quantity = item.CantTransferido,
            });
            productStockOrigen.Add(new ProductStock()
            {
                Id = string.Empty,
                WarehouseId = transferencia.WarehouseOriginId,
                ProductId = item.ProductId,
                Type = InventoryType.SALIDA,
                Quantity = item.CantTransferido,
            });
        });
        transferenciaDetails = await _productStockService.CalcularCantidadExistenteRestanteTransferenciaAsync(transferenciaDetails, transferencia.WarehouseOriginId);
        await _productStockService.CreateAsync(productStockDestino);
        await _productStockService.CreateAsync(productStockOrigen);
        await _transferenciaDetailService.DeleteManyAsync(transferencia.Id);
        await _transferenciaDetailService.InsertManyAsync(transferenciaDetails);
        transferencia.Status = InventoryStatus.VALIDADO;
        await _transferenciaService.UpdateAsync(transferencia.Id, transferencia);
        return transferencia;
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
        await _productStockService.CreateAsync(productStocks);
        await _ajusteInventarioDetailService.DeleteManyAsync(ajusteInventario.Id);
        await _ajusteInventarioDetailService.InsertManyAsync(ajusteInventarioDetails);
        ajusteInventario.Status = InventoryStatus.VALIDADO;
        await _ajusteInventarioService.UpdateAsync(ajusteInventario.Id, ajusteInventario);
        return ajusteInventario;
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
        await _productStockService.CreateAsync(productStocks);
        material.Status = InventoryStatus.VALIDADO;
        await _materialService.UpdateAsync(material.Id, material);
        return material;
    }

    public async Task ValidarInvoiceSale(string id)
    {
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(id);
        var productStocks = new List<ProductStock>();
        invoiceSaleDetails.ForEach(item =>
        {
            if (item.ControlStock == TipoControlStock.STOCK)
            {
                productStocks.Add(new ProductStock()
                {
                    Id = string.Empty,
                    WarehouseId = item.WarehouseId,
                    ProductId = item.CodProducto,
                    Type = InventoryType.SALIDA,
                    Quantity = (long)item.CtdUnidadItem,
                });
            }
        });
        if (productStocks.Count > 0)
            await _productStockService.CreateAsync(productStocks);
    }
}
