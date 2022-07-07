using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data.Models;
using Nebula.Data.Models.Cashier;
using Nebula.Data.Helpers;
using Nebula.Data.Services.Cashier;

namespace Nebula.Data.Services
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
                using (var session = await mongoClient.StartSessionAsync())
                {
                    session.StartTransaction();
                    try
                    {
                        var cargo = await GetAsync(model.ReceivableId);
                        model.ContactId = cargo.ContactId;
                        model.ContactName = cargo.ContactName;
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
                        await session.CommitTransactionAsync();
                    }
                    catch (Exception)
                    {
                        await session.AbortTransactionAsync();
                    }
                }
            }
        }

        public async Task RemoveAbonoAsync(Receivable abono)
        {
            using (var session = await mongoClient.StartSessionAsync())
            {
                session.StartTransaction();

                try
                {
                    var cargo = await GetAsync(abono.ReceivableId);
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
                    await session.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                }
            }
        }

        private CashierDetail GetCashierDetail(Receivable abono)
        {
            var cashierDetail = new CashierDetail();
            cashierDetail.Hour = DateTime.Now.ToString("HH:mm");
            cashierDetail.Contact = $"{abono.ContactId}:{abono.ContactName}";
            string document = abono.Document != "-" ? abono.Document.Split(":")[1].Trim() : "-";
            string remark = abono.Document != "-" ? $"COBRANZA, {document}" : abono.Remark;
            cashierDetail.Remark = remark;
            cashierDetail.TypeOperation = TypeOperationCaja.EntradaDeDinero;
            cashierDetail.FormaPago = abono.FormaPago;
            cashierDetail.Amount = abono.Abono;
            cashierDetail.CajaDiaria = abono.CajaDiaria.Split(':')[0].Trim();
            cashierDetail.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd");
            cashierDetail.Month = DateTime.Now.ToString("MM");
            cashierDetail.Year = DateTime.Now.ToString("yyyy");
            return cashierDetail;
        }
    }
}
