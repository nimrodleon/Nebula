using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Database.Dto.Cashier;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Cashier.Models;

namespace Nebula.Modules.Cashier;

public class CashierDetailService : CrudOperationService<CashierDetail>
{
    public CashierDetailService(IOptions<DatabaseSettings> options) : base(options)
    {
    }

    public async Task<List<CashierDetail>> GetListAsync(string id, string query)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.CajaDiaria, id),
            builder.Or(builder.Regex("Document", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("ContactName", new BsonRegularExpression(query.ToUpper(), "i")),
                builder.Regex("Remark", new BsonRegularExpression(query.ToUpper(), "i"))));
        return await _collection.Find(filter).ToListAsync();
    }

    private decimal CalcularTotalesCaja(List<CashierDetail> detalleCaja, string formaDePago)
    {
        var filter = detalleCaja.Where(x =>
            x.TypeOperation != TypeOperationCaja.SalidaDeDinero && x.FormaPago == formaDePago);
        return filter.Sum(x => x.Amount);
    }

    public async Task<ResumenCajaDto> GetResumenCaja(string cajaDiariaId)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.Eq(x => x.CajaDiaria, cajaDiariaId);
        List<CashierDetail> detalleCaja = await _collection.Find(filter).ToListAsync();
        decimal totalSalidas = detalleCaja.Where(x =>
                x.TypeOperation == TypeOperationCaja.SalidaDeDinero && x.FormaPago == FormaPago.Contado)
            .Sum(x => x.Amount);
        decimal totalContado = CalcularTotalesCaja(detalleCaja, FormaPago.Contado);
        ResumenCajaDto resumenCaja = new ResumenCajaDto
        {
            Yape = CalcularTotalesCaja(detalleCaja, FormaPago.Yape),
            Credito = CalcularTotalesCaja(detalleCaja, FormaPago.Credito),
            Contado = totalContado,
            Deposito = CalcularTotalesCaja(detalleCaja, FormaPago.Deposito),
            Salida = totalSalidas,
            MontoTotal = totalContado - totalSalidas
        };
        return resumenCaja;
    }

    public async Task<long> CountDocumentsAsync(string id) =>
        await _collection.CountDocumentsAsync(x => x.CajaDiaria == id);

    public async Task<List<CashierDetail>> GetEntradaSalidaAsync(string contactId, string month, string year)
    {
        var builder = Builders<CashierDetail>.Filter;
        var filter = builder.And(builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Month, month), builder.Eq(x => x.Year, year),
            builder.In("TypeOperation", new List<string>() { "ENTRADA_DE_DINERO", "SALIDA_DE_DINERO" }));
        return await _collection.Find(filter).ToListAsync();
    }
}
