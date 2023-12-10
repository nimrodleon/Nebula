using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common.Models;

namespace Nebula.Common;

public interface ICrudOperationService<T> where T : class, IGenericModel
{
    [Obsolete]
    Task<List<T>> GetAsync(string field, string? query, int limit = 12);
    Task<List<T>> GetFilteredAsync(string companyId, string[] fieldNames, string query = "", int limit = 12);
    Task<T> GetByIdAsync(string id);
    Task<T> GetByIdAsync(string companyId, string id);
    Task<T> CreateAsync(T obj);
    Task InsertManyAsync(List<T>? objList);
    Task<T> UpdateAsync(string id, T obj);
    Task RemoveAsync(string id);
    Task RemoveAsync(string companyId, string id);
}

public class CrudOperationService<T> : ICrudOperationService<T> where T : class, IGenericModel
{
    protected readonly IMongoCollection<T> _collection;

    public CrudOperationService(MongoDatabaseService mongoDatabase)
    {
        _collection = mongoDatabase.GetDatabase().GetCollection<T>(typeof(T).Name);
    }

    [Obsolete]
    public virtual async Task<List<T>> GetAsync(string field, string? query, int limit = 12)
    {
        var filter = Builders<T>.Filter.Empty;
        if (!string.IsNullOrWhiteSpace(query))
            filter = Builders<T>.Filter.Regex(field, new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<T>()
            .Descending("$natural")).Limit(limit).ToListAsync();
    }

    public virtual async Task<List<T>> GetFilteredAsync(string companyId, string[] fieldNames, string query = "", int limit = 12)
    {
        var builder = Builders<T>.Filter;
        var filter = builder.Eq("CompanyId", companyId);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var orFilters = new List<FilterDefinition<T>>();

            foreach (var fieldName in fieldNames)
            {
                var regexFilter = builder.Regex(fieldName, new BsonRegularExpression(query.ToUpper(), "i"));
                orFilters.Add(regexFilter);
            }

            var queryFilter = builder.Or(orFilters);
            filter &= queryFilter;
        }

        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<T>()
            .Descending("$natural")).Limit(limit).ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public virtual async Task<T> GetByIdAsync(string companyId, string id)
    {
        var filter = Builders<T>.Filter.And(
            Builders<T>.Filter.Eq("CompanyId", companyId),
            Builders<T>.Filter.Eq(x => x.Id, id));
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task<T> CreateAsync(T obj)
    {
        obj.Id = string.Empty;
        await _collection.InsertOneAsync(obj);
        return obj;
    }

    public virtual async Task InsertManyAsync(List<T>? objList) =>
        await _collection.InsertManyAsync(objList);

    public virtual async Task<T> UpdateAsync(string id, T obj)
    {
        await _collection.ReplaceOneAsync(x => x.Id == id, obj);
        return obj;
    }

    public virtual async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

    public virtual async Task RemoveAsync(string companyId, string id)
    {
        var filter = Builders<T>.Filter.And(
                Builders<T>.Filter.Eq(x => x.Id, id),
                Builders<T>.Filter.Eq("CompanyId", companyId)
            );
        await _collection.DeleteOneAsync(filter);
    }
}
