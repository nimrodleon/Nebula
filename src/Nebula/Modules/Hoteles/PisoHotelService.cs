using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Hoteles.Models;

namespace Nebula.Modules.Hoteles;

public interface IPisoHotelService : ICrudOperationService<PisoHotel>
{
    Task<List<PisoHotel>> GetPisosByArrIds(List<string> arrIds);
}

public class PisoHotelService : CrudOperationService<PisoHotel>, IPisoHotelService
{
    public PisoHotelService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<List<PisoHotel>> GetPisosByArrIds(List<string> arrIds)
    {
        var filter = Builders<PisoHotel>.Filter.In(x => x.Id, arrIds);
        return await _collection.Find(filter).ToListAsync();
    }

}
