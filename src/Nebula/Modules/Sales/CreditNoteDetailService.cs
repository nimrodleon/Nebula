using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Database;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales;

public class CreditNoteDetailService : CrudOperationService<CreditNoteDetail>
{
    public CreditNoteDetailService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<CreditNoteDetail>> GetListAsync(string creditNoteId) =>
       await _collection.Find(x => x.CreditNoteId == creditNoteId).ToListAsync();
}
