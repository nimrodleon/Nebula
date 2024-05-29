using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Notes;

public interface ICreditNoteDetailService : ICrudOperationService<CreditNoteDetail>
{
    Task<List<CreditNoteDetail>> GetListAsync(string companyId, string creditNoteId);
}

public class CreditNoteDetailService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<CreditNoteDetail>(mongoDatabase), ICreditNoteDetailService
{
    public async Task<List<CreditNoteDetail>> GetListAsync(string companyId, string creditNoteId) =>
       await _collection.Find(x => x.CompanyId == companyId && x.CreditNoteId == creditNoteId).ToListAsync();
}
