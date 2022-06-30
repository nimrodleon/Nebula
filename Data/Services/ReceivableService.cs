using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data.Models;

namespace Nebula.Data.Services
{
    public class ReceivableService : CrudOperationService<Receivable>
    {
        public ReceivableService(IOptions<DatabaseSettings> options) : base(options) { }

        public async Task<List<Receivable>> GetListAsync(string month, string year, string status)
        {
            var filter = Builders<Receivable>.Filter;
            var query = filter.And(filter.Eq(x => x.Year, year), filter.Eq(x => x.Month, month), filter.Eq(x => x.Status, status));
            return await _collection.Find(query).ToListAsync();
        }
    }
}
