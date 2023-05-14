using Nebula.Database.Helpers;
using Nebula.Database.Models.Common;
using Nebula.Plugins.Inventory.Models;
using Nebula.Plugins.Inventory.Stock.Dto;

namespace Nebula.Plugins.Inventory.Stock.Helpers;

public class StockListRequestParams
{
    public List<ProductStock> Stocks { get; set; } = new List<ProductStock>();
    public List<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    public List<ProductLote> Lotes { get; set; } = new List<ProductLote>();
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
        return _params.Product.HasLotes ? GetProductStockListWithLotes() : GetStockListWithoutLotes();
    }

    /// <summary>
    /// Obtener el stock del producto para cada almacén con cada lote de producto.
    /// </summary>
    private List<ProductStockInfoDto> GetProductStockListWithLotes()
    {
        var result = new List<ProductStockInfoDto>();
        _params.Warehouses.ForEach(warehouse =>
        {
            _params.Lotes.ForEach(lote =>
            {
                result.Add(new ProductStockInfoDto
                {
                    WarehouseId = warehouse.Id,
                    WarehouseName = warehouse.Name,
                    ProductId = _params.Product.Id,
                    ProductLoteId = lote.Id,
                    ProductLoteName = lote.LotNumber,
                    Quantity = GetQuantityWithLotes(warehouse.Id, lote.Id),
                    ExpirationDate = lote.ExpirationDate
                });
            });
        });
        return result;
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
    private long GetQuantityWithoutLotes(string warehouseId)
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
    private long GetQuantityWithLotes(string warehouseId, string loteId)
    {
        var stockList = _params.Stocks.Where(x => x.WarehouseId.Equals(warehouseId)
                        && x.ProductLoteId.Equals(loteId)).ToList();
        return CalcularExistenciaStock(stockList);
    }

    /// <summary>
    /// Calcula la existencia de stock a partir de la lista de documentos de stock.
    /// </summary>
    /// <param name="stocks">La lista de documentos de stock</param>
    /// <returns>La existencia de stock calculada</returns>
    private long CalcularExistenciaStock(List<ProductStock> stocks)
    {
        long totalEntradas = 0;
        long totalSalidas = 0;

        foreach (var stock in stocks)
        {
            if (stock.Type.Equals(InventoryType.ENTRADA))
            {
                totalEntradas += stock.Quantity;
            }
            else if (stock.Type.Equals(InventoryType.SALIDA))
            {
                totalSalidas += stock.Quantity;
            }
        }

        return totalEntradas - totalSalidas;
    }
}
