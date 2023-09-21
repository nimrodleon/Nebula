using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Invoices;

public interface IDetallePagoSaleService : ICrudOperationService<DetallePagoSale>
{
    Task<List<DetallePagoSale>> GetListAsync(string id);
    Task CreateManyAsync(List<DetallePagoSale> detallePago);
}

public class DetallePagoSaleService : CrudOperationService<DetallePagoSale>, IDetallePagoSaleService
{
    public DetallePagoSaleService(MongoDatabaseService mongoDatabase) : base(mongoDatabase) { }

    public async Task<List<DetallePagoSale>> GetListAsync(string id) =>
        await _collection.Find(x => x.InvoiceSale == id).ToListAsync();

    public async Task CreateManyAsync(List<DetallePagoSale> detallePago) =>
        await _collection.InsertManyAsync(detallePago);
}
