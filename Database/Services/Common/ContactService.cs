using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Common;

namespace Nebula.Database.Services.Common;

public class ContactService : CrudOperationService<Contact>
{
    public ContactService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<Contact> GetContactByDocumentAsync(string document) =>
        await _collection.Find(x => x.Document == document).FirstOrDefaultAsync();
}
