using Microsoft.Extensions.Options;
using Nebula.Common;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth;

public interface IRoleService : ICrudOperationService<Roles>
{

}

public class RoleService : CrudOperationService<Roles>, IRoleService
{
    public RoleService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

}
