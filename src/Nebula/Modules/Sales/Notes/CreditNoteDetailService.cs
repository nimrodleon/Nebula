using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Notes;

public interface ICreditNoteDetailService : ICrudOperationService<CreditNoteDetail>
{
    Task<List<CreditNoteDetail>> GetListAsync(string creditNoteId);
}

public class CreditNoteDetailService : CrudOperationService<CreditNoteDetail>, ICreditNoteDetailService
{
    public CreditNoteDetailService(IOptions<DatabaseSettings> options) : base(options) { }

    public async Task<List<CreditNoteDetail>> GetListAsync(string creditNoteId) =>
       await _collection.Find(x => x.CreditNoteId == creditNoteId).ToListAsync();
}
