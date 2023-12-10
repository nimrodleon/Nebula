using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Account;

public interface IWarehouseService : ICrudOperationService<Warehouse>
{
    Task<List<Warehouse>> GetAllAsync(string companyId);
}

public class WarehouseService : CrudOperationService<Warehouse>, IWarehouseService
{
    public WarehouseService(MongoDatabaseService mongoDatabase) : base(mongoDatabase) { }

    public async Task<List<Warehouse>> GetAllAsync(string companyId) =>
        await _collection.Find(x => x.CompanyId == companyId).ToListAsync();
}
