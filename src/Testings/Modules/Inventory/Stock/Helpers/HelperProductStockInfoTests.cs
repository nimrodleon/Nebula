using Nebula.Database.Helpers;
using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Stock.Helpers;
using Nebula.Modules.Products.Models;

namespace Testings.Plugins.Inventory.Stock.Helpers;

[TestClass]
public class HelperProductStockInfoTests
{
    [TestMethod]
    public void GetStockListWithoutLotes_Should_Return_Correct_Quantity()
    {
        // Arrange
        var stocks = new List<ProductStock>
        {
            new ProductStock { WarehouseId = "1", ProductLoteId = "", Type = InventoryType.ENTRADA, Quantity = 10 },
            new ProductStock { WarehouseId = "1", ProductLoteId = "", Type = InventoryType.SALIDA, Quantity = 5 },
            new ProductStock { WarehouseId = "2", ProductLoteId = "", Type = InventoryType.ENTRADA, Quantity = 15 },
            new ProductStock { WarehouseId = "2", ProductLoteId = "", Type = InventoryType.SALIDA, Quantity = 10 }
        };

        var warehouses = new List<Warehouse>
        {
            new Warehouse { Id = "1", Name = "Warehouse 1" },
            new Warehouse { Id = "2", Name = "Warehouse 2" }
        };

        var requestParams = new StockListRequestParams
        {
            Stocks = stocks,
            Warehouses = warehouses,
            Product = new Product { Id = "1", HasLotes = false }
        };

        var helper = new HelperProductStockInfo(requestParams);

        // Act
        var result = helper.GetStockList();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("1", result[0].WarehouseId);
        Assert.AreEqual("Warehouse 1", result[0].WarehouseName);
        Assert.AreEqual("1", result[0].ProductId);
        Assert.AreEqual(5, result[0].Quantity);
        Assert.AreEqual("2", result[1].WarehouseId);
        Assert.AreEqual("Warehouse 2", result[1].WarehouseName);
        Assert.AreEqual("1", result[1].ProductId);
        Assert.AreEqual(5, result[1].Quantity);
    }

    [TestMethod]
    public void GetProductStockListWithLotes_Should_Return_Correct_Quantity()
    {
        // Arrange
        var stocks = new List<ProductStock>
        {
            new ProductStock { WarehouseId = "1", ProductLoteId = "1", Type = InventoryType.ENTRADA, Quantity = 10 },
            new ProductStock { WarehouseId = "1", ProductLoteId = "1", Type = InventoryType.SALIDA, Quantity = 5 },
            new ProductStock { WarehouseId = "1", ProductLoteId = "2", Type = InventoryType.ENTRADA, Quantity = 15 },
            new ProductStock { WarehouseId = "1", ProductLoteId = "2", Type = InventoryType.SALIDA, Quantity = 10 }
        };

        var warehouses = new List<Warehouse>
        {
            new Warehouse { Id = "1", Name = "Warehouse 1" },
            new Warehouse { Id = "2", Name = "Warehouse 2" }
        };

        var lotes = new List<ProductLote>
        {
            new ProductLote {Id="1", LotNumber="L001"},
            new ProductLote {Id="2", LotNumber="L002"}
        };

        var requestParams = new StockListRequestParams
        {
            Stocks = stocks,
            Warehouses = warehouses,
            Lotes = lotes,
            Product = new Product { Id = "1", HasLotes = true }
        };

        var helper = new HelperProductStockInfo(requestParams);

        // Act
        var result = helper.GetStockList();

        // Assert
        Assert.AreEqual(4, result.Count);
        Assert.AreEqual("1", result[0].WarehouseId);
        Assert.AreEqual("Warehouse 1", result[0].WarehouseName);
        Assert.AreEqual("1", result[0].ProductId);
        Assert.AreEqual("1", result[0].ProductLoteId);
        Assert.AreEqual(5, result[0].Quantity);
        Assert.AreEqual("1", result[1].WarehouseId);
        Assert.AreEqual("Warehouse 1", result[1].WarehouseName);
        Assert.AreEqual("1", result[1].ProductId);
        Assert.AreEqual("2", result[1].ProductLoteId);
        Assert.AreEqual(5, result[1].Quantity);
        Assert.AreEqual("2", result[2].WarehouseId);
        Assert.AreEqual("Warehouse 2", result[2].WarehouseName);
        Assert.AreEqual("1", result[2].ProductId);
        Assert.AreEqual("1", result[2].ProductLoteId);
        Assert.AreEqual(0, result[2].Quantity);
        Assert.AreEqual("2", result[3].WarehouseId);
        Assert.AreEqual("Warehouse 2", result[3].WarehouseName);
        Assert.AreEqual("1", result[3].ProductId);
        Assert.AreEqual("2", result[3].ProductLoteId);
        Assert.AreEqual(0, result[3].Quantity);
    }
}
