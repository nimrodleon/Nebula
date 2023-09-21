using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common.Models;

namespace Nebula.Common;

public interface ICrudOperationService<T> where T : class, IGenericModel
{
    Task<List<T>> GetAsync(string field, string? query, int limit = 25);
    Task<T> GetByIdAsync(string id);
    Task<T> CreateAsync(T obj);
    Task InsertManyAsync(List<T> objList);
    Task<T> UpdateAsync(string id, T obj);
    Task RemoveAsync(string id);
}

public class CrudOperationService<T> : ICrudOperationService<T> where T : class, IGenericModel
{
    protected readonly IMongoCollection<T> _collection;

    public CrudOperationService(MongoDatabaseService mongoDatabase)
    {
        _collection = mongoDatabase.GetDatabase().GetCollection<T>(typeof(T).Name);
    }

    public virtual async Task<List<T>> GetAsync(string field, string? query, int limit = 25)
    {
        var filter = Builders<T>.Filter.Empty;
        if (!string.IsNullOrWhiteSpace(query))
            filter = Builders<T>.Filter.Regex(field, new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public virtual async Task<T> CreateAsync(T obj)
    {
        obj.Id = string.Empty;
        await _collection.InsertOneAsync(obj);
        return obj;
    }

    public virtual async Task InsertManyAsync(List<T> objList) =>
        await _collection.InsertManyAsync(objList);

    public virtual async Task<T> UpdateAsync(string id, T obj)
    {
        await _collection.ReplaceOneAsync(x => x.Id == id, obj);
        return obj;
    }

    public virtual async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
