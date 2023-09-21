using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Common.Dto;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Notes;

public interface ITributoCreditNoteService : ICrudOperationService<TributoCreditNote>
{
    Task<List<TributoCreditNote>> GetListAsync(string creditNoteId);
    Task<List<TributoCreditNote>> GetTributosMensual(DateQuery date);
}

public class TributoCreditNoteService : CrudOperationService<TributoCreditNote>, ITributoCreditNoteService
{
    public TributoCreditNoteService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<List<TributoCreditNote>> GetListAsync(string creditNoteId) =>
        await _collection.Find(x => x.CreditNoteId == creditNoteId).ToListAsync();

    /// <summary>
    /// Obtener Lista de Tributos Mensual.
    /// </summary>
    /// <param name="date">Datos mes y año</param>
    /// <returns>Lista de tributos</returns>
    public async Task<List<TributoCreditNote>> GetTributosMensual(DateQuery date)
    {
        var builder = Builders<TributoCreditNote>.Filter;
        var filter = builder.And(builder.Eq(x => x.Month, date.Month),
            builder.Eq(x => x.Year, date.Year));
        return await _collection.Find(filter).ToListAsync();
    }
}
