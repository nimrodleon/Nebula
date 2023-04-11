using Microsoft.Extensions.Options;
using Nebula.Database;
using Nebula.Database.Services;
using Nebula.Plugins.Taller.Models;

namespace Nebula.Plugins.Taller.Services;

/// <summary>
/// Servicio Orden de Reparación.
/// </summary>
public class TallerRepairOrderService : CrudOperationService<TallerRepairOrder>
{
    public TallerRepairOrderService(IOptions<DatabaseSettings> options) : base(options)
    {
    }
}
