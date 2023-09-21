using Nebula.Common;
using Nebula.Modules.Configurations.Models;

namespace Nebula.Modules.Configurations.Warehouses;

public interface IInvoiceSerieService : ICrudOperationService<InvoiceSerie>
{

}

public class InvoiceSerieService : CrudOperationService<InvoiceSerie>, IInvoiceSerieService
{
    public InvoiceSerieService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }
}
