using Microsoft.Extensions.Options;
using Nebula.Database;
using Nebula.Database.Services;
using Nebula.Plugins.Taller.Models;

namespace Nebula.Plugins.Taller.Services;

/// <summary>
/// Servicio Item de la Orden de Reparación.
/// </summary>
public class TallerItemRepairOrderService : CrudOperationService<TallerItemRepairOrder>
{
    public TallerItemRepairOrderService(IOptions<DatabaseSettings> options) : base(options)
    {
    }
}
