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

/// <summary>
///  Clase de ayuda que se encarga de procesar y calcular la existencia de stock
///  de un producto en cada almacén, con o sin lotes de productos registrados.
/// </summary>
public class HelperProductStockInfo
{
    private readonly StockListRequestParams _params;

    public HelperProductStockInfo(StockListRequestParams requestParams)
    {
        _params = requestParams;
    }

    /// <summary>
    /// Obtener la lista de stock
    /// </summary>
    /// <returns>Lista de stock</returns>
    public List<ProductStockInfoDto> GetStockList()
    {
        return GetStockListWithoutLotes();
    }

    /// <summary>
    /// Obtener el Stock del producto sin lotes de productos registrados en cada almacén
    /// </summary>
    private List<ProductStockInfoDto> GetStockListWithoutLotes()
    {
        var result = new List<ProductStockInfoDto>();
        _params.Warehouses.ForEach(item =>
        {
            result.Add(new ProductStockInfoDto
            {
                WarehouseId = item.Id,
                WarehouseName = item.Name,
                ProductId = _params.Product.Id,
                ProductLoteId = string.Empty,
                ProductLoteName = string.Empty,
                Quantity = GetQuantityWithoutLotes(item.Id),
                ExpirationDate = string.Empty
            });
        });
        return result;
    }

    /// <summary>
    /// Obtiene la cantidad de existencia de un producto sin tener en cuenta los lotes del producto.
    /// </summary>
    /// <param name="warehouseId">El identificador del almacén</param>
    /// <returns>La cantidad de existencia sin lotes</returns>
    private decimal GetQuantityWithoutLotes(string warehouseId)
    {
        var stockList = _params.Stocks.Where(x => x.WarehouseId.Equals(warehouseId)).ToList();
        return CalcularExistenciaStock(stockList);
    }

    /// <summary>
    /// Obtiene la cantidad de existencia de un producto específico en un almacén y lote específicos.
    /// </summary>
    /// <param name="warehouseId">Identificador del almacén</param>
    /// <param name="loteId">Identificador del lote de productos</param>
    /// <returns>Cantidad de existencia con lote del producto</returns>
    private decimal GetQuantityWithLotes(string warehouseId, string loteId)
    {
        var stockList = _params.Stocks.Where(x => x.WarehouseId.Equals(warehouseId)).ToList();
        return CalcularExistenciaStock(stockList);
    }

    /// <summary>
    /// Calcula la existencia de stock a partir de la lista de documentos de stock.
    /// </summary>
    /// <param name="stocks">La lista de documentos de stock</param>
    /// <returns>La existencia de stock calculada</returns>
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
