using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Contacts.Models;

namespace Nebula.Modules.Contacts;

public interface IContactService : ICrudOperationService<Contact>
{
    Task<Contact> GetContactByDocumentAsync(string companyId, string document);
    Task<List<Contact>> GetContactsAsync(string companyId, string query = "", int limit = 25);
}

public class ContactService : CrudOperationService<Contact>, IContactService
{
    public ContactService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
        var indexKeys = Builders<Contact>.IndexKeys.Combine(
            Builders<Contact>.IndexKeys.Ascending(x => x.CompanyId),
            Builders<Contact>.IndexKeys.Ascending(x => x.Document));
        var indexOptions = new CreateIndexOptions { Unique = true };
        var model = new CreateIndexModel<Contact>(indexKeys, indexOptions);
        _collection.Indexes.CreateOne(model);
    }

    public async Task<Contact> GetContactByDocumentAsync(string companyId, string document)
    {
        var builder = Builders<Contact>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Document, document));
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<Contact>> GetContactsAsync(string companyId, string query = "", int limit = 25)
    {
        var builder = Builders<Contact>.Filter;
        var filter = builder.Eq(x => x.CompanyId, companyId);
        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Or(builder.Regex("Document", new BsonRegularExpression(query, "i")),
                builder.Regex("Name", new BsonRegularExpression(query.ToUpper(), "i")));
        }
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }
}
