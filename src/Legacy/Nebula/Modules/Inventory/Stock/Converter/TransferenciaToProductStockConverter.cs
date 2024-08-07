using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Stock.Dto;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class TransferenciaToProductStockConverter
{
    private readonly TransferenciaDto _transferenciaDto;

    public TransferenciaToProductStockConverter(TransferenciaDto transferenciaDto)
    {
        _transferenciaDto = transferenciaDto;
    }

    public TransferenciaResult Convertir()
    {
        var productStockDestino = new List<ProductStock>();
        var productStockOrigen = new List<ProductStock>();
        var result = new TransferenciaResult();
        _transferenciaDto.TransferenciaDetails.ForEach(item =>
        {
            result.ProductStockDestino.Add(new ProductStock()
            {
                Id = string.Empty,
                CompanyId = _transferenciaDto.Transferencia.CompanyId.Trim(),
                WarehouseId = _transferenciaDto.Transferencia.WarehouseTargetId,
                ProductId = item.ProductId,
                TransactionType = TransactionType.ENTRADA,
                Quantity = item.CantTransferido,
            });
            result.ProductStockOrigen.Add(new ProductStock()
            {
                Id = string.Empty,
                CompanyId = _transferenciaDto.Transferencia.CompanyId.Trim(),
                WarehouseId = _transferenciaDto.Transferencia.WarehouseOriginId,
                ProductId = item.ProductId,
                TransactionType = TransactionType.SALIDA,
                Quantity = item.CantTransferido,
            });
        });
        return result;
    }
}
