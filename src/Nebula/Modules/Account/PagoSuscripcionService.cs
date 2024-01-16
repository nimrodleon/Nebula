using Nebula.Common;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Account;

public interface IPagoSuscripcionService : ICrudOperationService<PagoSuscripcion>
{

}

public class PagoSuscripcionService : CrudOperationService<PagoSuscripcion>, IPagoSuscripcionService
{
    public PagoSuscripcionService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

}
