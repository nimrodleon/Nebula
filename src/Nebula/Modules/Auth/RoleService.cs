using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth;

public interface IRoleService : ICrudOperationService<Roles>
{
    Task<Roles> GetByNombreAsync(string name);
}

public class RoleService : CrudOperationService<Roles>, IRoleService
{
    public RoleService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<Roles> GetByNombreAsync(string name)
    {
        var filter = Builders<Roles>.Filter.Eq(x => x.Nombre, name);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
