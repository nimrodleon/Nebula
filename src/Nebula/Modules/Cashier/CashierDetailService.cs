using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Cashier.Dto;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Cashier.Models;

namespace Nebula.Modules.Cashier;

public interface ICashierDetailService : ICrudOperationService<CashierDetail>
{
    Task<List<CashierDetail>> GetListAsync(string companyId, string id, string query);

    Task<List<CashierDetail>> GetDetalleCajaDiariaAsync(string companyId, string id, string query = "", int page = 1,
        int pageSize = 12);

    Task<long> GetTotalDetalleCajaDiariaAsync(string companyId, string id, string query = "");
    Task<ResumenCajaDto> GetResumenCaja(string companyId, string cajaDiariaId);
    Task<long> CountDocumentsAsync(string companyId, string id);
    Task<List<CashierDetail>> GetEntradaSalidaAsync(string companyId, string contactId, string month, string year);
}

public class CashierDetailService(MongoDatabaseService mongoDatabase)
    : CrudOperationService<CashierDetail>(mongoDatabase), ICashierDetailService
{
    public async Task<List<CashierDetail>> GetListAsync(string companyId, string id, string query)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.CajaDiariaId, id),
            builder.Or(builder.Regex("Document", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("ContactName", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("Remark", new BsonRegularExpression(query.ToUpper(), "i"))));
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<List<CashierDetail>> GetDetalleCajaDiariaAsync(string companyId, string id, string query = "",
        int page = 1, int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.CajaDiariaId, id),
            builder.Or(builder.Regex("Document", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("ContactName", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("Remark", new BsonRegularExpression(query.ToUpper(), "i"))));
        return await _collection.Find(filter).Sort(new SortDefinitionBuilder<CashierDetail>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();
    }

    public async Task<long> GetTotalDetalleCajaDiariaAsync(string companyId, string id, string query = "")
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.CajaDiariaId, id),
            builder.Or(builder.Regex("Document", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("ContactName", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("Remark", new BsonRegularExpression(query.ToUpper(), "i"))));
        return await _collection.Find(filter).CountDocumentsAsync();
    }

    private decimal CalcularTotalesCaja(List<CashierDetail> detalleCaja, string formaDePago)
    {
        var filter = detalleCaja.Where(x =>
            x.TypeOperation != TipoOperationCaja.SalidaDeDinero && x.FormaPago == formaDePago);
        return filter.Sum(x => x.Amount);
    }

    public async Task<ResumenCajaDto> GetResumenCaja(string companyId, string cajaDiariaId)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.CajaDiariaId, cajaDiariaId));
        List<CashierDetail> detalleCaja = await _collection.Find(filter).ToListAsync();
        decimal totalSalidas = detalleCaja
            .Where(x => x.TypeOperation == TipoOperationCaja.SalidaDeDinero && x.FormaPago == MetodosPago.Contado)
            .Sum(x => x.Amount);
        decimal totalContado = CalcularTotalesCaja(detalleCaja, MetodosPago.Contado);
        ResumenCajaDto resumenCaja = new ResumenCajaDto
        {
            Yape = CalcularTotalesCaja(detalleCaja, MetodosPago.Yape),
            Credito = CalcularTotalesCaja(detalleCaja, MetodosPago.Credito),
            Contado = totalContado,
            Deposito = CalcularTotalesCaja(detalleCaja, MetodosPago.Deposito),
            Salida = totalSalidas,
            MontoTotal = totalContado - totalSalidas
        };
        return resumenCaja;
    }

    public async Task<long> CountDocumentsAsync(string companyId, string id) =>
        await _collection.CountDocumentsAsync(x => x.CompanyId == companyId && x.CajaDiariaId == id);

    public async Task<List<CashierDetail>> GetEntradaSalidaAsync(string companyId, string contactId, string month,
        string year)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Month, month), builder.Eq(x => x.Year, year),
            builder.In("TypeOperation", new List<string>() { "ENTRADA_DE_DINERO", "SALIDA_DE_DINERO" }));
        return await _collection.Find(filter).ToListAsync();
    }
}
