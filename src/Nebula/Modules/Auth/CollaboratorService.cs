using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth;

public interface ICollaboratorService : ICrudOperationService<Collaborator>
{
    Task<List<Collaborator>> GetCollaborationsByUserIdAsync(string userId);
}

public class CollaboratorService : CrudOperationService<Collaborator>, ICollaboratorService
{
    public CollaboratorService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<List<Collaborator>> GetCollaborationsByUserIdAsync(string userId)
    {
        var filter = Builders<Collaborator>.Filter.Eq(x => x.UserId, userId);
        var collaborations = await _collection.Find(filter).ToListAsync();
        return collaborations;
    }
}
