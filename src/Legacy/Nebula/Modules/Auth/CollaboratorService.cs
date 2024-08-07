using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth;

public interface ICollaboratorService : ICrudOperationService<Collaborator>
{
    Task<List<Collaborator>> GetCollaborationsByCompanyId(string companyId);
    Task<List<Collaborator>> GetCollaborationsByUserIdAsync(string userId);
}

public class CollaboratorService : CrudOperationService<Collaborator>, ICollaboratorService
{
    public CollaboratorService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
        var indexKeys = Builders<Collaborator>.IndexKeys
            .Combine(Builders<Collaborator>.IndexKeys.Ascending(x => x.CompanyId),
            Builders<Collaborator>.IndexKeys.Ascending(x => x.UserId));
        var indexOptions = new CreateIndexOptions { Unique = true };
        var model = new CreateIndexModel<Collaborator>(indexKeys, indexOptions);
        _collection.Indexes.CreateOne(model);
    }

    public async Task<List<Collaborator>> GetCollaborationsByCompanyId(string companyId)
    {
        var filter = Builders<Collaborator>.Filter.Eq(x => x.CompanyId, companyId.Trim());
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<Collaborator>> GetCollaborationsByUserIdAsync(string userId)
    {
        var filter = Builders<Collaborator>.Filter.Eq(x => x.UserId, userId);
        var collaborations = await _collection.Find(filter).ToListAsync();
        return collaborations;
    }
}
