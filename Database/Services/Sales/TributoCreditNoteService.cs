using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Services.Sales;

public class TributoCreditNoteService : CrudOperationService<TributoCreditNote>
{
    public TributoCreditNoteService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<TributoCreditNote>> GetListAsync(string creditNoteId) =>
       await _collection.Find(x => x.CreditNoteId == creditNoteId).ToListAsync();
}
