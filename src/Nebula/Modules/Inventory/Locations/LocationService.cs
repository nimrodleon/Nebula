using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Stock;

namespace Nebula.Modules.Inventory.Locations;

public interface ILocationService : ICrudOperationService<Location>
{
    Task<List<Location>> GetByWarehouseIdAsync(string companyId, string id);
    Task<RespLocationDetailStock> GetLocationDetailStocksAsync(string companyId, string id, bool reponer = false);
}

public class LocationService : CrudOperationService<Location>, ILocationService
{
    private readonly IProductStockService _productStockService;
    private readonly ILocationDetailService _locationDetailService;

    public LocationService(MongoDatabaseService mongoDatabase,
        IProductStockService productStockService,
        ILocationDetailService locationDetailService) : base(mongoDatabase)
    {
        _productStockService = productStockService;
        _locationDetailService = locationDetailService;
    }

    public async Task<List<Location>> GetByWarehouseIdAsync(string companyId, string id)
    {
        var builder = Builders<Location>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.WarehouseId, id));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<RespLocationDetailStock> GetLocationDetailStocksAsync(string companyId, string id, bool reponer = false)
    {
        var location = await GetByIdAsync(companyId, id);
        var locationDetails = await _locationDetailService.GetListAsync(companyId, location.Id);
        var productArrId = new List<string>();
        // obtener lista de identificadores.
        locationDetails.ForEach(item => productArrId.Add(item.ProductId));
        var productStocks = await _productStockService.GetProductStockByProductIdsAsync(companyId, location.WarehouseId, productArrId);
        var locationDetailStocks = new List<LocationItemStockDto>();
        locationDetails.ForEach(item =>
        {
            var products = productStocks.Where(x => x.ProductId == item.ProductId).ToList();
            var itemStock = new LocationItemStockDto();
            itemStock.ProductId = item.ProductId;
            itemStock.Description = item.ProductName;
            itemStock.QuantityMax = item.QuantityMax;
            itemStock.QuantityMin = item.QuantityMin;
            // calcular existencia de productos.
            var entradasList = products.Where(x => x.TransactionType == "ENTRADA").ToList();
            var salidasList = products.Where(x => x.TransactionType == "SALIDA").ToList();
            decimal totalEntradas = entradasList.Sum(x => x.Quantity);
            decimal totalSalidas = salidasList.Sum(x => x.Quantity);
            itemStock.Stock = totalEntradas - totalSalidas;
            if (!reponer)
                locationDetailStocks.Add(itemStock);
            else if (itemStock.Stock <= item.QuantityMin)
                locationDetailStocks.Add(itemStock);
        });
        return new RespLocationDetailStock()
        {
            Location = location,
            LocationDetailStocks = locationDetailStocks
        };
    }
}
