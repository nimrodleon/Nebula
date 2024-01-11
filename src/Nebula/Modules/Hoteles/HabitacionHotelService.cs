using Nebula.Common;
using Nebula.Modules.Hoteles.Models;

namespace Nebula.Modules.Hoteles;

public interface IHabitacionHotelService : ICrudOperationService<HabitacionHotel>
{

}

public class HabitacionHotelService : CrudOperationService<HabitacionHotel>, IHabitacionHotelService
{
    public HabitacionHotelService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }
}
