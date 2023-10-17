using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Inventory.Stock.Validations;

namespace Nebula.Modules.Inventory.Stock;

public interface IValidateStockService
{
    Task<InventoryNotas> ValidarNotas(string companyId, string id);
    Task<Transferencia> ValidarTransferencia(string companyId, string id);
    Task<AjusteInventario> ValidarAjusteInventario(string companyId, string id);
    Task<Material> ValidarMaterial(string companyId, string id);
    Task ValidarInvoiceSale(string companyId, string id);
}

public class ValidateStockService : IValidateStockService
{
    private readonly INotaStockValidator _inventoryNotasStockValidator;
    private readonly IInventoryTransferenciaStockValidator _inventoryTransferenciaStockValidator;
    private readonly IAjusteInventarioStockValidator _ajusteInventarioStockValidator;
    private readonly IInventoryMaterialStockValidator _inventoryMaterialStockValidator;
    private readonly IInvoiceSaleStockValidator _invoiceSaleStockValidator;

    public ValidateStockService(
        INotaStockValidator inventoryNotasStockValidator,
        IInventoryTransferenciaStockValidator inventoryTransferenciaStockValidator,
        IAjusteInventarioStockValidator ajusteInventarioStockValidator,
        IInventoryMaterialStockValidator inventoryMaterialStockValidator,
        IInvoiceSaleStockValidator invoiceSaleStockValidator)
    {
        _inventoryNotasStockValidator = inventoryNotasStockValidator;
        _inventoryTransferenciaStockValidator = inventoryTransferenciaStockValidator;
        _ajusteInventarioStockValidator = ajusteInventarioStockValidator;
        _inventoryMaterialStockValidator = inventoryMaterialStockValidator;
        _invoiceSaleStockValidator = invoiceSaleStockValidator;
    }

    public async Task<InventoryNotas> ValidarNotas(string companyId, string id)
    {
        return await _inventoryNotasStockValidator.ValidarNotas(companyId, id);
    }

    public async Task<Transferencia> ValidarTransferencia(string companyId, string id)
    {
        return await _inventoryTransferenciaStockValidator.ValidarTransferencia(companyId, id);
    }

    public async Task<AjusteInventario> ValidarAjusteInventario(string companyId, string id)
    {
        return await _ajusteInventarioStockValidator.ValidarAjusteInventario(companyId, id);
    }

    public async Task<Material> ValidarMaterial(string companyId, string id)
    {
        return await _inventoryMaterialStockValidator.ValidarMaterial(companyId, id);
    }

    public async Task ValidarInvoiceSale(string companyId, string id)
    {
        await _invoiceSaleStockValidator.ValidarInvoiceSale(companyId, id);
    }
}
