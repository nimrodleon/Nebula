using Nebula.Modules.Account.Models;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Stock.Dto;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Inventory.Stock.Helpers;

public class StockListRequestParams
{
    public List<ProductStock> Stocks { get; set; } = new List<ProductStock>();
    public List<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    public Product Product { get; set; } = new Product();
}

public class HelperProductStockInfo
{
    private readonly StockListRequestParams _params;

    public HelperProductStockInfo(StockListRequestParams requestParams)
    {
        _params = requestParams;
    }

    public List<ProductStockInfoDto> GetStockInfo()
    {
        var result = new List<ProductStockInfoDto>();
        _params.Warehouses.ForEach(item =>
        {
            result.Add(new ProductStockInfoDto
            {
                WarehouseId = item.Id,
                WarehouseName = item.Name,
                ProductId = _params.Product.Id,
                Quantity = GetQuantityByWarehouseId(item.Id)
            });
        });
        return result;
    }

    private decimal GetQuantityByWarehouseId(string warehouseId)
    {
        var stockList = _params.Stocks.Where(x => x.WarehouseId.Equals(warehouseId)).ToList();
        return CalcularExistenciaStock(stockList);
    }

    private decimal CalcularExistenciaStock(List<ProductStock> stocks)
    {
        decimal totalEntradas = 0;
        decimal totalSalidas = 0;

        foreach (var stock in stocks)
        {
            if (stock.TransactionType.Equals(TransactionType.ENTRADA))
            {
                totalEntradas += stock.Quantity;
            }
            else if (stock.TransactionType.Equals(TransactionType.SALIDA))
            {
                totalSalidas += stock.Quantity;
            }
        }

        return totalEntradas - totalSalidas;
    }
}
