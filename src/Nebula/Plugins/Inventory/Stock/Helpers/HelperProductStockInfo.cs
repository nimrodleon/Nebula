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

public class HelperProductStockInfo
{
    private readonly StockListRequestParams _params;

    public HelperProductStockInfo(StockListRequestParams requestParams)
    {
        _params = requestParams;
    }

    public List<ProductStockInfoDto> GetStockList()
    {
        return _params.Product.HasLotes ? GetProductStockListWithLotes() : GetStockListWithoutLotes();
    }

    /// <summary>
    /// Obtener Stock con lotes de producto.
    /// </summary>
    private List<ProductStockInfoDto> GetProductStockListWithLotes()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Obtener Stock con sin lotes de producto.
    /// </summary>
    private List<ProductStockInfoDto> GetStockListWithoutLotes()
    {
        throw new NotImplementedException();
    }
}
