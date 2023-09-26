using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Transferencias;

namespace Nebula.Modules.Inventory.Stock.Validations;

public interface IInventoryTransferenciaStockValidator
{
    Task<Transferencia> ValidarTransferencia(string companyId, string id);
}

public class InventoryTransferenciaStockValidator : IInventoryTransferenciaStockValidator
{
    private readonly IProductStockService _productStockService;
    private readonly ITransferenciaService _transferenciaService;
    private readonly ITransferenciaDetailService _transferenciaDetailService;

    public InventoryTransferenciaStockValidator(IProductStockService productStockService,
        ITransferenciaService transferenciaService,
        ITransferenciaDetailService transferenciaDetailService)
    {
        _productStockService = productStockService;
        _transferenciaService = transferenciaService;
        _transferenciaDetailService = transferenciaDetailService;
    }

    public async Task<Transferencia> ValidarTransferencia(string companyId, string id)
    {
        var transferencia = await _transferenciaService.GetByIdAsync(companyId, id);
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
        await _productStockService.CreateManyAsync(productStockDestino);
        await _productStockService.CreateManyAsync(productStockOrigen);
        await _transferenciaDetailService.DeleteManyAsync(transferencia.Id);
        await _transferenciaDetailService.InsertManyAsync(transferenciaDetails);
        transferencia.Status = InventoryStatus.VALIDADO;
        await _transferenciaService.UpdateAsync(transferencia.Id, transferencia);
        return transferencia;
    }
}
