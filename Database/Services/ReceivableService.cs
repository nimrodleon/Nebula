using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Models;
using Nebula.Database.Models.Cashier;
using Nebula.Database.Helpers;
using Nebula.Database.Services.Cashier;

namespace Nebula.Database.Services
{
    public class ReceivableService : CrudOperationService<Receivable>
    {
        public ReceivableService(IOptions<DatabaseSettings> options) : base(options) { }

        public async Task<List<Receivable>> GetListAsync(string month, string year, string status)
        {
            var filter = Builders<Receivable>.Filter;
            var query = filter.And(filter.Eq(x => x.Year, year), filter.Eq(x => x.Month, month),
                filter.In("Status", new List<string>() { status, "-" }), filter.In("Type", new List<string>() { "CARGO", "ABONO" }));
            return await _collection.Find(query).ToListAsync();
        }

        public async Task<List<Receivable>> GetAbonosAsync(string id)
        {
            var filter = Builders<Receivable>.Filter;
            var query = filter.And(filter.Eq(x => x.ReceivableId, id), filter.Eq(x => x.Type, "ABONO"));
            return await _collection.Find(query).ToListAsync();
        }

        public async Task<long> GetTotalAbonosAsync(string id)
        {
            var filter = Builders<Receivable>.Filter;
            var query = filter.And(filter.Eq(x => x.ReceivableId, id), filter.Eq(x => x.Type, "ABONO"));
            return await _collection.Find(query).CountDocumentsAsync();
        }

        public async Task CreateAsync(CashierDetailService cashierDetailService, Receivable model)
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
                var abonos = await GetAbonosAsync(model.ReceivableId);
                var totalAbonos = abonos.Sum(x => x.Abono) + model.Abono;
                if ((cargo.Cargo - totalAbonos) > 0)
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

        public async Task RemoveAbonoAsync(Receivable abono)
        {
            var cargo = await GetByIdAsync(abono.ReceivableId);
            var abonos = await GetAbonosAsync(cargo.Id);
            var totalAbonos = abonos.Sum(x => x.Abono) - abono.Abono;
            if ((cargo.Cargo - totalAbonos) > 0)
            {
                await RemoveAsync(abono.Id);
                cargo.Status = "PENDIENTE";
                await UpdateAsync(cargo.Id, cargo);
            }
            else
            {
                await RemoveAsync(abono.Id);
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
    }
}
