using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Cashier;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Finanzas.Dto;

namespace Nebula.Modules.Finanzas;

public interface IReceivableService : ICrudOperationService<Receivable>
{
    Task<List<Receivable>> GetListAsync(string companyId, ReceivableRequestParams param);
    Task<List<Receivable>> GetAbonosAsync(string companyId, string id);
    Task<long> GetTotalAbonosAsync(string companyId, string id);
    Task CreateAsync(ICashierDetailService cashierDetailService, Receivable model);
    Task RemoveAbonoAsync(string companyId, Receivable abono);
    Task<List<Receivable>> GetReceivablesByContactId(string companyId,
        string contactId, ReceivableRequestParams requestParam);
    Task<List<Receivable>> GetReceivablesByContactId(string companyId, string contactId, string year);
}

public class ReceivableService : CrudOperationService<Receivable>, IReceivableService
{
    public ReceivableService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<List<Receivable>> GetListAsync(string companyId, ReceivableRequestParams param)
    {
        var builder = Builders<Receivable>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Year, param.Year), builder.Eq(x => x.Month, param.Month),
            builder.In("Status", new List<string>() { param.Status, "-" }),
            builder.In("Type", new List<string>() { "CARGO", "ABONO" }));
        var receivables = await _collection.Find(filter).ToListAsync();
        List<Receivable> cuentasPorCobrar = receivables.Where(x => x.Type == "CARGO").ToList();
        List<Receivable> listaDeAbonos = receivables.Where(x => x.Type == "ABONO").ToList();
        cuentasPorCobrar.ForEach(item => { item.Saldo = CalcularSaldoCargo(listaDeAbonos, item); });
        return cuentasPorCobrar;
    }

    public async Task<List<Receivable>> GetAbonosAsync(string companyId, string id)
    {
        var filter = Builders<Receivable>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.ReceivableId, id), filter.Eq(x => x.Type, "ABONO"));
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<long> GetTotalAbonosAsync(string companyId, string id)
    {
        var filter = Builders<Receivable>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.ReceivableId, id), filter.Eq(x => x.Type, "ABONO"));
        return await _collection.Find(query).CountDocumentsAsync();
    }

    public async Task CreateAsync(ICashierDetailService cashierDetailService, Receivable model)
    {
        if (model.Type.Equals("CARGO")) await CreateAsync(model);

        if (model.Type.Equals("ABONO"))
        {
            var cargo = await GetByIdAsync(model.ReceivableId);
            model.ContactId = cargo.ContactId;
            model.ContactName = cargo.ContactName;
            model.InvoiceSale = cargo.InvoiceSale;
            model.DocType = cargo.DocType;
            model.Document = cargo.Document;
            model.Month = cargo.Month;
            model.Year = cargo.Year;
            var abonos = await GetAbonosAsync(model.CompanyId, model.ReceivableId);
            var totalAbonos = abonos.Sum(x => x.Abono) + model.Abono;
            if (cargo.Cargo - totalAbonos > 0)
            {
                await CreateAsync(model);
                if (model.CajaDiaria != "-")
                    await cashierDetailService.CreateAsync(GetCashierDetail(model));
            }
            else
            {
                await CreateAsync(model);
                if (model.CajaDiaria != "-")
                    await cashierDetailService.CreateAsync(GetCashierDetail(model));
                cargo.Status = "COBRADO";
                await UpdateAsync(cargo.Id, cargo);
            }
        }
    }

    public async Task RemoveAbonoAsync(string companyId, Receivable abono)
    {
        var cargo = await GetByIdAsync(companyId, abono.ReceivableId);
        var abonos = await GetAbonosAsync(companyId, cargo.Id);
        var totalAbonos = abonos.Sum(x => x.Abono) - abono.Abono;
        if (cargo.Cargo - totalAbonos > 0)
        {
            await RemoveAsync(companyId, abono.Id);
            cargo.Status = "PENDIENTE";
            await UpdateAsync(cargo.Id, cargo);
        }
        else
        {
            await RemoveAsync(companyId, abono.Id);
        }
    }

    private CashierDetail GetCashierDetail(Receivable abono)
    {
        var cashierDetail = new CashierDetail();
        cashierDetail.Hour = DateTime.Now.ToString("HH:mm");
        cashierDetail.ContactId = abono.ContactId;
        cashierDetail.ContactName = abono.ContactName;
        string document = abono.InvoiceSale != "-" ? abono.Document : "-";
        string remark = abono.InvoiceSale != "-" ? $"COBRANZA, {document}" : abono.Remark;
        cashierDetail.Remark = remark;
        cashierDetail.TypeOperation = TypeOperationCaja.EntradaDeDinero;
        cashierDetail.FormaPago = abono.FormaPago;
        cashierDetail.Amount = abono.Abono;
        cashierDetail.CajaDiaria = abono.CajaDiaria;
        cashierDetail.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd");
        cashierDetail.Month = DateTime.Now.ToString("MM");
        cashierDetail.Year = DateTime.Now.ToString("yyyy");
        return cashierDetail;
    }

    private decimal CalcularSaldoCargo(List<Receivable> listaDeAbonos, Receivable cuenta)
    {
        List<Receivable> filter = listaDeAbonos.Where(x => x.ReceivableId == cuenta.Id).ToList();
        decimal abono = filter.Sum(x => x.Abono);
        return cuenta.Cargo - abono;
    }

    public async Task<List<Receivable>> GetReceivablesByContactId(string companyId,
        string contactId, ReceivableRequestParams requestParam)
    {
        var builder = Builders<Receivable>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Year, requestParam.Year), builder.Eq(x => x.Month, requestParam.Month),
            builder.In("Status", new List<string>() { requestParam.Status, "-" }),
            builder.In("Type", new List<string>() { "CARGO", "ABONO" }));
        List<Receivable> receivables = await _collection.Find(filter).ToListAsync();
        List<Receivable> cuentasPorCobrar = receivables.Where(x => x.Type == "CARGO").ToList();
        List<Receivable> listaDeAbonos = receivables.Where(x => x.Type == "ABONO").ToList();
        cuentasPorCobrar.ForEach(item => { item.Saldo = CalcularSaldoCargo(listaDeAbonos, item); });
        return cuentasPorCobrar;
    }

    public async Task<List<Receivable>> GetReceivablesByContactId(string companyId, string contactId, string year)
    {
        var builder = Builders<Receivable>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Year, year), builder.In("Status", new List<string>() { "PENDIENTE", "-" }),
            builder.In("Type", new List<string>() { "CARGO", "ABONO" }));
        List<Receivable> receivables = await _collection.Find(filter).ToListAsync();
        List<Receivable> cuentasPorCobrar = receivables.Where(x => x.Type == "CARGO").ToList();
        List<Receivable> listaDeAbonos = receivables.Where(x => x.Type == "ABONO").ToList();
        cuentasPorCobrar.ForEach(item => { item.Saldo = CalcularSaldoCargo(listaDeAbonos, item); });
        return cuentasPorCobrar;
    }
}
