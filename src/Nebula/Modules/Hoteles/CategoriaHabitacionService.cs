using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Hoteles.Models;

namespace Nebula.Modules.Hoteles;

public interface ICategoriaHabitacionService : ICrudOperationService<CategoriaHabitacion>
{
    Task<List<CategoriaHabitacion>> GetCategoriasByArrIds(List<string> arrIds);
}

public class CategoriaHabitacionService : CrudOperationService<CategoriaHabitacion>, ICategoriaHabitacionService
{
    public CategoriaHabitacionService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<List<CategoriaHabitacion>> GetCategoriasByArrIds(List<string> arrIds)
    {
        var filter = Builders<CategoriaHabitacion>.Filter.In(x => x.Id, arrIds);
        return await _collection.Find(filter).ToListAsync();
    }
}
