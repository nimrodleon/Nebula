using MongoDB.Driver;
using Nebula.Common;
using Nebula.Common.Dto;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Invoices;

public interface ITributoSaleService : ICrudOperationService<TributoSale>
{
    Task<List<TributoSale>> GetListAsync(string companyId, string invoiceSaleId);
    Task<List<TributoSale>> GetTributosMensual(string companyId, DateQuery date);
    Task CreateManyAsync(List<TributoSale> tributoSales);
    Task RemoveManyAsync(string companyId, string id);
}

public class TributoSaleService : CrudOperationService<TributoSale>, ITributoSaleService
{
    public TributoSaleService(MongoDatabaseService mongoDatabase) : base(mongoDatabase)
    {
    }

    /// <summary>
    /// Retorna la lista de tributos de un comprobante.
    /// </summary>
    /// <param name="companyId">identificador a la empresa que pertenece</param>
    /// <param name="invoiceSaleId">identificador del comprobante</param>
    /// <returns>Lista de tributos</returns>
    public async Task<List<TributoSale>> GetListAsync(string companyId, string invoiceSaleId)
    {
        var builder = Builders<TributoSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.InvoiceSaleId, invoiceSaleId));
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Obtener Lista de Tributos Mensual.
    /// </summary>
    /// <param name="date">Datos mes y año</param>
    /// <returns>Lista de tributos</returns>
    public async Task<List<TributoSale>> GetTributosMensual(string companyId, DateQuery date)
    {
        var builder = Builders<TributoSale>.Filter;
        var filter = builder.And(builder.Eq(x => x.CompanyId, companyId),
            builder.Eq(x => x.Month, date.Month), builder.Eq(x => x.Year, date.Year));
        return await _collection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Guardar todos los tributos.
    /// </summary>
    /// <param name="tributoSales">Lista de tributos</param>
    /// <returns>Los resultados de la operación de inserción</returns>
    public async Task CreateManyAsync(List<TributoSale> tributoSales) =>
        await _collection.InsertManyAsync(tributoSales);

    /// <summary>
    /// Borra los tributos que coincidan con los parametros de la función.
    /// </summary>
    /// <param name="companyId">Identificador de la empresa</param>
    /// <param name="invoiceSaleId">Identificador del comprobante</param>
    /// <returns>Los resultados de la operación de eliminación</returns>
    public async Task RemoveManyAsync(string companyId, string invoiceSaleId) =>
        await _collection.DeleteManyAsync(x => x.CompanyId == companyId
            && x.InvoiceSaleId == invoiceSaleId);
}
