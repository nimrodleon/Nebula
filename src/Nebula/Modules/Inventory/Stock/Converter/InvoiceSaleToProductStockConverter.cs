using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Converter;

public class InvoiceSaleToProductStockConverter
{
    private readonly InvoiceSaleStockDto _invoiceSaleStockDto;

    public InvoiceSaleToProductStockConverter(InvoiceSaleStockDto invoiceSaleStockDto)
    {
        _invoiceSaleStockDto = invoiceSaleStockDto;
    }

    public List<ProductStock> Convertir()
    {
        var productStocks = new List<ProductStock>();
        _invoiceSaleStockDto.InvoiceSaleDetails.ForEach(item =>
        {
            if (item.RecordType == "PRODUCTO")
            {
                productStocks.Add(new ProductStock()
                {
                    Id = string.Empty,
                    CompanyId = _invoiceSaleStockDto.InvoiceSale.CompanyId.Trim(),
                    WarehouseId = item.WarehouseId,
                    ProductId = item.CodProducto,
                    TransactionType = TransactionType.SALIDA,
                    Quantity = item.Cantidad,
                });
            }
        });
        return productStocks;
    }
}
