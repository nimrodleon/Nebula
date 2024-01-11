using Nebula.Common;
using Nebula.Modules.Hoteles.Models;

namespace Nebula.Modules.Hoteles;

public interface ICategoriaHabitacionService : ICrudOperationService<CategoriaHabitacion>
{

}

public class CategoriaHabitacionService : CrudOperationService<CategoriaHabitacion>, ICategoriaHabitacionService
{
    public CategoriaHabitacionService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }
}
