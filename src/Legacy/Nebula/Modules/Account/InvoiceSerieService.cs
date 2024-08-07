using Nebula.Common;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Account;

public interface IInvoiceSerieService : ICrudOperationService<InvoiceSerie>
{

}

public class InvoiceSerieService : CrudOperationService<InvoiceSerie>, IInvoiceSerieService
{
    public InvoiceSerieService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }
}
