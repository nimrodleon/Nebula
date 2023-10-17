using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class TransferenciaToProductStockConverter
{
    public object Convertir(TransferenciaDto dto)
    {
        var productStockDestino = new List<ProductStock>();
        var productStockOrigen = new List<ProductStock>();
        dto.TransferenciaDetails.ForEach(item =>
        {
            productStockDestino.Add(new ProductStock()
            {
                Id = string.Empty,
                CompanyId = dto.Transferencia.CompanyId.Trim(),
                WarehouseId = dto.Transferencia.WarehouseTargetId,
                ProductId = item.ProductId,
                TransactionType = TransactionType.ENTRADA,
                Quantity = item.CantTransferido,
            });
            productStockOrigen.Add(new ProductStock()
            {
                Id = string.Empty,
                CompanyId = dto.Transferencia.CompanyId.Trim(),
                WarehouseId = dto.Transferencia.WarehouseOriginId,
                ProductId = item.ProductId,
                TransactionType = TransactionType.SALIDA,
                Quantity = item.CantTransferido,
            });
        });
        return new { productStockOrigen, productStockDestino };
    }
}
