using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Dto.Inventory;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory;

public class LocationService : CrudOperationService<Location>
{
    private readonly ProductStockService _productStockService;
    private readonly LocationDetailService _locationDetailService;

    public LocationService(IOptions<DatabaseSettings> options,
        ProductStockService productStockService,
        LocationDetailService locationDetailService) : base(options)
    {
        _productStockService = productStockService;
        _locationDetailService = locationDetailService;
    }

    public async Task<List<Location>> GetByWarehouseIdAsync(string id)
    {
        var builder = Builders<Location>.Filter;
        var filter = builder.Eq(x => x.WarehouseId, id);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<LocationDetailStockDto>> GetLocationDetailStocksAsync(string id)
    {
        var location = await GetAsync(id);
        var locationDetails = await _locationDetailService.GetListAsync(location.Id);
        var productArrId = new List<string>();
        // obtener lista de identificadores.
        locationDetails.ForEach(item => productArrId.Add(item.ProductId));
        var productStocks = await _productStockService.GetProductStockByWarehouseIdAsync(location.WarehouseId, productArrId);
        var locationDetailStocks = new List<LocationDetailStockDto>();
        locationDetails.ForEach(item =>
        {
            var products = productStocks.Where(x => x.ProductId == item.ProductId).ToList();
            var itemStock = new LocationDetailStockDto();
            itemStock.ProductId = item.ProductId;
            itemStock.Description = item.ProductName;
            itemStock.QuantityMax = item.QuantityMax;
            itemStock.QuantityMin = item.QuantityMin;
            // calcular existencia de productos.
            var entradasList = products.Where(x => x.Type == "ENTRADA").ToList();
            var salidasList = products.Where(x => x.Type == "SALIDA").ToList();
            long totalEntradas = entradasList.Sum(x => x.Quantity);
            long totalSalidas = salidasList.Sum(x => x.Quantity);
            itemStock.Stock = totalEntradas - totalSalidas;
            locationDetailStocks.Add(itemStock);
        });
        return locationDetailStocks;
    }
}
