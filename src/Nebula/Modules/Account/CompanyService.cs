using Nebula.Common;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Account;

public interface ICompanyService : ICrudOperationService<Company>
{

}

public class CompanyService : CrudOperationService<Company>, ICompanyService
{
    public CompanyService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }
}
