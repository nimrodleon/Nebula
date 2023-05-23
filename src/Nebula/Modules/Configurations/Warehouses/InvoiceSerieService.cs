using Microsoft.Extensions.Options;
using Nebula.Common;
using Nebula.Modules.Configurations.Models;

namespace Nebula.Modules.Configurations.Warehouses;

public class InvoiceSerieService : CrudOperationService<InvoiceSerie>
{
    public InvoiceSerieService(IOptions<DatabaseSettings> options) : base(options)
    {
    }
}
