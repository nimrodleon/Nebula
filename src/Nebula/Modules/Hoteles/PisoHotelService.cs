using Nebula.Common;
using Nebula.Modules.Hoteles.Models;

namespace Nebula.Modules.Hoteles;

public interface IPisoHotelService : ICrudOperationService<PisoHotel>
{

}

public class PisoHotelService : CrudOperationService<PisoHotel>, IPisoHotelService
{
    public PisoHotelService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

}
