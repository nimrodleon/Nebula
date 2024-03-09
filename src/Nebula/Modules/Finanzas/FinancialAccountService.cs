using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Cashier;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Cashier.Helpers;
using Nebula.Modules.Finanzas.Schemas;
using Nebula.Modules.Contacts.Models;

namespace Nebula.Modules.Finanzas;

public interface IFinancialAccountService : ICrudOperationService<FinancialAccount>
{
    Task<List<FinancialAccount>> GetListAsync(string companyId, CuentaPorCobrarMensualParamSchema param, int page = 1, int pageSize = 12);
    Task<long> GetTotalCargosAsync(string companyId, CuentaPorCobrarMensualParamSchema param);
    Task<List<FinancialAccount>> GetAbonosAsync(string companyId, string id);
    Task<long> GetTotalAbonosAsync(string companyId, string id);
    Task CreateAsync(ICashierDetailService cashierDetailService, FinancialAccount model);
    Task RemoveAbonoAsync(string companyId, FinancialAccount abono);
    Task<List<FinancialAccount>> GetCargosClienteAnual(string companyId, CuentaPorCobrarClienteAnualParamSchema param);
    Task<List<FinancialAccount>> GetReceivablesByContactId(string companyId,
        string contactId, CuentaPorCobrarMensualParamSchema requestParam);
    Task<List<FinancialAccount>> GetReceivablesByContactId(string companyId, string contactId, string year);
    Task<List<FinancialAccount>> GetPendientesPorCobrar(string companyId);
}

public class FinancialAccountService : CrudOperationService<FinancialAccount>, IFinancialAccountService
{
    public FinancialAccountService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<List<FinancialAccount>> GetListAsync(string companyId, CuentaPorCobrarMensualParamSchema param, int page = 1, int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;
        var builder = Builders<FinancialAccount>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Year, param.Year), builder.Eq(x => x.Month, param.Month),
            builder.In("Status", new List<string>() { param.Status, "-" }),
            builder.In("Type", new List<string>() { "CARGO", "ABONO" }));
        var receivables = await _collection.Find(filter).Sort(new SortDefinitionBuilder<FinancialAccount>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();
        List<FinancialAccount> cuentasPorCobrar = receivables.Where(x => x.Type == "CARGO").ToList();
        List<FinancialAccount> listaDeAbonos = receivables.Where(x => x.Type == "ABONO").ToList();
        cuentasPorCobrar.ForEach(item => { item.Saldo = CalcularSaldoCargo(listaDeAbonos, item); });
        return cuentasPorCobrar;
    }

    public async Task<long> GetTotalCargosAsync(string companyId, CuentaPorCobrarMensualParamSchema param)
    {
        var builder = Builders<FinancialAccount>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Year, param.Year), builder.Eq(x => x.Month, param.Month),
            builder.In("Status", new List<string>() { param.Status, "-" }),
            builder.Eq(x => x.Type, "CARGO"));
        return await _collection.Find(filter).CountDocumentsAsync();
    }

    public async Task<List<FinancialAccount>> GetAbonosAsync(string companyId, string id)
    {
        var filter = Builders<FinancialAccount>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.ReceivableId, id), filter.Eq(x => x.Type, "ABONO"));
        return await _collection.Find(query).ToListAsync();
    }

    public async Task<long> GetTotalAbonosAsync(string companyId, string id)
    {
        var filter = Builders<FinancialAccount>.Filter;
        var query = filter.And(filter.Eq(x => x.CompanyId, companyId),
            filter.Eq(x => x.ReceivableId, id), filter.Eq(x => x.Type, "ABONO"));
        return await _collection.Find(query).CountDocumentsAsync();
    }

    public async Task CreateAsync(ICashierDetailService cashierDetailService, FinancialAccount model)
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

    public async Task RemoveAbonoAsync(string companyId, FinancialAccount abono)
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

    private CashierDetail GetCashierDetail(FinancialAccount abono)
    {
        var cashierDetail = new CashierDetail();
        cashierDetail.CompanyId = abono.CompanyId.Trim();
        cashierDetail.Hour = DateTime.Now.ToString("HH:mm");
        cashierDetail.ContactId = abono.ContactId;
        cashierDetail.ContactName = abono.ContactName;
        string document = abono.InvoiceSale != "-" ? abono.Document : "-";
        string remark = abono.InvoiceSale != "-" ? $"COBRANZA, {document}" : abono.Remark;
        cashierDetail.Remark = remark;
        cashierDetail.TypeOperation = TipoOperationCaja.EntradaDeDinero;
        cashierDetail.FormaPago = abono.FormaPago;
        cashierDetail.Amount = abono.Abono;
        cashierDetail.CajaDiariaId = abono.CajaDiaria;
        cashierDetail.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd");
        cashierDetail.Month = DateTime.Now.ToString("MM");
        cashierDetail.Year = DateTime.Now.ToString("yyyy");
        return cashierDetail;
    }

    private decimal CalcularSaldoCargo(List<FinancialAccount> listaDeAbonos, FinancialAccount cuenta)
    {
        List<FinancialAccount> filter = listaDeAbonos.Where(x => x.ReceivableId == cuenta.Id).ToList();
        decimal abono = filter.Sum(x => x.Abono);
        return cuenta.Cargo - abono;
    }

    public async Task<List<FinancialAccount>> GetReceivablesByContactId(string companyId,
        string contactId, CuentaPorCobrarMensualParamSchema requestParam)
    {
        var builder = Builders<FinancialAccount>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId), builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Year, requestParam.Year), builder.Eq(x => x.Month, requestParam.Month),
            builder.In("Status", new List<string>() { requestParam.Status, "-" }),
            builder.In("Type", new List<string>() { "CARGO", "ABONO" }));
        List<FinancialAccount> receivables = await _collection.Find(filter).ToListAsync();
        List<FinancialAccount> cuentasPorCobrar = receivables.Where(x => x.Type == "CARGO").ToList();
        List<FinancialAccount> listaDeAbonos = receivables.Where(x => x.Type == "ABONO").ToList();
        cuentasPorCobrar.ForEach(item => { item.Saldo = CalcularSaldoCargo(listaDeAbonos, item); });
        return cuentasPorCobrar;
    }

    public async Task<List<FinancialAccount>> GetCargosClienteAnual(string companyId,
        CuentaPorCobrarClienteAnualParamSchema param)
    {
        var builder = Builders<FinancialAccount>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.ContactId, param.ContactId),
            builder.Eq(x => x.Year, param.Year),
            builder.In("Status", new List<string>() { param.Status, "-" }),
            builder.In("Type", new List<string>() { "CARGO", "ABONO" }));
        List<FinancialAccount> receivables = await _collection.Find(filter).ToListAsync();
        List<FinancialAccount> cuentasPorCobrar = receivables.Where(x => x.Type == "CARGO").ToList();
        List<FinancialAccount> listaDeAbonos = receivables.Where(x => x.Type == "ABONO").ToList();
        cuentasPorCobrar.ForEach(item => { item.Saldo = CalcularSaldoCargo(listaDeAbonos, item); });
        return cuentasPorCobrar;
    }

    public async Task<List<FinancialAccount>> GetReceivablesByContactId(string companyId, string contactId, string year)
    {
        var builder = Builders<FinancialAccount>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.ContactId, contactId),
            builder.Eq(x => x.Year, year), builder.In("Status", new List<string>() { "PENDIENTE", "-" }),
            builder.In("Type", new List<string>() { "CARGO", "ABONO" }));
        List<FinancialAccount> receivables = await _collection.Find(filter).ToListAsync();
        List<FinancialAccount> cuentasPorCobrar = receivables.Where(x => x.Type == "CARGO").ToList();
        List<FinancialAccount> listaDeAbonos = receivables.Where(x => x.Type == "ABONO").ToList();
        cuentasPorCobrar.ForEach(item => { item.Saldo = CalcularSaldoCargo(listaDeAbonos, item); });
        return cuentasPorCobrar;
    }

    public async Task<List<FinancialAccount>> GetPendientesPorCobrar(string companyId)
    {
        var builder = Builders<FinancialAccount>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
             builder.In("Status", new List<string>() { "PENDIENTE", "-" }),
            builder.In("Type", new List<string>() { "CARGO", "ABONO" }));
        List<FinancialAccount> receivables = await _collection.Find(filter).ToListAsync();
        List<FinancialAccount> cuentasPorCobrar = receivables.Where(x => x.Type == "CARGO").ToList();
        List<FinancialAccount> listaDeAbonos = receivables.Where(x => x.Type == "ABONO").ToList();
        cuentasPorCobrar.ForEach(item => { item.Saldo = CalcularSaldoCargo(listaDeAbonos, item); });
        return cuentasPorCobrar;
    }
}
