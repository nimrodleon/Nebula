using Nebula.Common;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth;

public interface ICollaboratorService : ICrudOperationService<Collaborator>
{

}

public class CollaboratorService : CrudOperationService<Collaborator>, ICollaboratorService
{
    public CollaboratorService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }
}
