using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Auth.Models;

namespace Nebula.Modules.Auth;

public interface IUserService : ICrudOperationService<User>
{
    Task<List<User>> GetListAsync(string? query, int limit = 25);
    Task<User> GetByUserNameAsync(string userName);
}

public class UserService : CrudOperationService<User>, IUserService
{
    private readonly IRoleService _roleService;

    public UserService(IOptions<DatabaseSettings> options, IRoleService roleService) : base(options)
    {
        _roleService = roleService;
    }

    public async Task<List<User>> GetListAsync(string? query, int limit = 25)
    {
        var filter = Builders<User>.Filter.Empty;
        if (!string.IsNullOrEmpty(query))
            filter = Builders<User>.Filter.Regex("UserName", new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }

    //public override async Task<User> GetByIdAsync(string id)
    //{
    //    var user = await base.GetByIdAsync(id);
    //    var role = await _roleService.GetByIdAsync(user.RolesId);
    //    user.Role = role.Nombre;
    //    return user;
    //}

    public async Task<User> GetByUserNameAsync(string userName)
    {
        var filter = Builders<User>.Filter.Eq(x => x.UserName, userName);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
