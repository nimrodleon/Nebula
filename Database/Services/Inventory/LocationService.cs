using Microsoft.Extensions.Options;
using Nebula.Database.Models.Inventory;

namespace Nebula.Database.Services.Inventory
{
    public class LocationService : CrudOperationService<Location>
    {
        public LocationService(IOptions<DatabaseSettings> options) : base(options) { }
    }
}
