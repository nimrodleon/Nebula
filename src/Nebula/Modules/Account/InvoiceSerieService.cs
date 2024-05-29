using Nebula.Common;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Account;

public interface IInvoiceSerieService : ICrudOperationService<InvoiceSerie>
{

}

public class InvoiceSerieService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<InvoiceSerie>(mongoDatabase), IInvoiceSerieService;
