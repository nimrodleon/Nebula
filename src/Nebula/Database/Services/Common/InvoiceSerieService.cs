using Microsoft.Extensions.Options;
using Nebula.Database.Models.Common;

namespace Nebula.Database.Services.Common;

public class InvoiceSerieService : CrudOperationService<InvoiceSerie>
{
    public InvoiceSerieService(IOptions<DatabaseSettings> options) : base(options)
    {
    }
}
