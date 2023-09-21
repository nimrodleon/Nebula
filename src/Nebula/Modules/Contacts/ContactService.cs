using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Contacts.Models;

namespace Nebula.Modules.Contacts;

public interface IContactService : ICrudOperationService<Contact>
{
    Task<Contact> GetContactByDocumentAsync(string document);
    Task<List<Contact>> GetContactsAsync(string query = "", int limit = 25);
}

public class ContactService : CrudOperationService<Contact>, IContactService
{
    public ContactService(MongoDatabaseService mongoDatabase) : base(mongoDatabase) { }

    public async Task<Contact> GetContactByDocumentAsync(string document) =>
        await _collection.Find(x => x.Document == document).FirstOrDefaultAsync();

    public async Task<List<Contact>> GetContactsAsync(string query = "", int limit = 25)
    {
        var builder = Builders<Contact>.Filter;
        var filter = builder.Or(builder.Regex("Document", new BsonRegularExpression(query, "i")),
            builder.Regex("Name", new BsonRegularExpression(query.ToUpper(), "i")));
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }
}
