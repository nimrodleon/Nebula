using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Notas;
using Nebula.Modules.Inventory.Stock.Converter;
using Nebula.Modules.Inventory.Stock.Helpers;
using Nebula.Modules.Inventory.Transferencias;
using Nebula.Modules.Sales.Comprobantes.Dto;

namespace Nebula.Modules.Inventory.Stock;

public interface IValidateStockService
{
    Task<InventoryNoteDto> ValidarNotas(string companyId, string InventoryNotasId);
    Task<TransferenciaDto> ValidarTransferencia(string companyId, string transferenciaId);
    Task<AjusteInventarioDto> ValidarAjusteInventario(string companyId, string ajusteInventarioId);
    Task<MaterialDto> ValidarMaterial(string companyId, string materialId);
    Task ValidarInvoiceSale(InvoiceSaleAndDetails model);
}

public class ValidateStockService(
    IProductStockService productStockService,
    IInventoryNotasService inventoryNotasService,
    IInventoryNotasDetailService inventoryNotasDetailService,
    ITransferenciaService transferenciaService,
    ITransferenciaDetailService transferenciaDetailService,
    IAjusteInventarioService ajusteInventarioService,
    IAjusteInventarioDetailService ajusteInventarioDetailService,
    IMaterialService materialService,
    IMaterialDetailService materialDetailService)
    : IValidateStockService
{
    public async Task<InventoryNoteDto> ValidarNotas(string companyId, string InventoryNotasId)
    {
        var dto = new InventoryNoteDto();
        dto.InventoryNotas = await inventoryNotasService.GetByIdAsync(companyId, InventoryNotasId);
        dto.InventoryNotasDetail = await inventoryNotasDetailService.GetListAsync(companyId, dto.InventoryNotas.Id);
        var productStocks = new InventoryNoteToProductStockConverter(dto).Convertir();
        await productStockService.CreateManyAsync(productStocks);
        dto.InventoryNotas.Status = InventoryStatus.VALIDADO;
        await inventoryNotasService.ReplaceOneAsync(dto.InventoryNotas.Id, dto.InventoryNotas);
        return dto;
    }

    public async Task<TransferenciaDto> ValidarTransferencia(string companyId, string transferenciaId)
    {
        var dto = new TransferenciaDto();
        dto.Transferencia = await transferenciaService.GetByIdAsync(companyId, transferenciaId);
        dto.TransferenciaDetails = await transferenciaDetailService.GetListAsync(companyId, dto.Transferencia.Id);

        var productArrId = new List<string>();
        dto.TransferenciaDetails.ForEach(item => productArrId.Add(item.ProductId));
        var productStocks = await productStockService.GetProductStockByProductIdsAsync(companyId, dto.Transferencia.WarehouseOriginId, productArrId);
        dto.TransferenciaDetails = new TransferenciaCalculator(productStocks, dto.TransferenciaDetails).CalcularCantidadesRestante(companyId);

        var result = new TransferenciaToProductStockConverter(dto).Convertir();
        await productStockService.CreateManyAsync(result.ProductStockOrigen);
        await productStockService.CreateManyAsync(result.ProductStockDestino);

        await transferenciaDetailService.DeleteManyAsync(companyId, dto.Transferencia.Id);
        await transferenciaDetailService.InsertManyAsync(dto.TransferenciaDetails);

        dto.Transferencia.Status = InventoryStatus.VALIDADO;
        await transferenciaService.ReplaceOneAsync(dto.Transferencia.Id, dto.Transferencia);
        return dto;
    }

    public async Task<AjusteInventarioDto> ValidarAjusteInventario(string companyId, string ajusteInventarioId)
    {
        var dto = new AjusteInventarioDto();
        dto.AjusteInventario = await ajusteInventarioService.GetByIdAsync(companyId, ajusteInventarioId);
        dto.AjusteInventarioDetails = await ajusteInventarioDetailService.GetListAsync(companyId, dto.AjusteInventario.Id);

        var productArrId = new List<string>();
        dto.AjusteInventarioDetails.ForEach(item => productArrId.Add(item.ProductId));
        var productStocks = await productStockService.GetProductStockByProductIdsAsync(companyId, dto.AjusteInventario.WarehouseId, productArrId);
        dto.AjusteInventarioDetails = new AjusteInventarioCalculator(productStocks, dto.AjusteInventarioDetails).CalcularCantidadExistente(companyId);
        await productStockService.DeleteProductStockByProductIdsAsync(companyId, dto.AjusteInventario.WarehouseId, productArrId);

        var newProductStocks = new AjusteInventarioToProductStockConverter(dto).Convertir();
        await productStockService.CreateManyAsync(newProductStocks);
        await ajusteInventarioDetailService.DeleteManyAsync(companyId, dto.AjusteInventario.Id);
        await ajusteInventarioDetailService.InsertManyAsync(dto.AjusteInventarioDetails);

        dto.AjusteInventario.Status = InventoryStatus.VALIDADO;
        await ajusteInventarioService.ReplaceOneAsync(dto.AjusteInventario.Id, dto.AjusteInventario);
        return dto;
    }

    public async Task<MaterialDto> ValidarMaterial(string companyId, string materialId)
    {
        var dto = new MaterialDto();
        dto.Material = await materialService.GetByIdAsync(companyId, materialId);
        dto.MaterialDetails = await materialDetailService.GetListAsync(companyId, dto.Material.Id);
        var productStocks = new MaterialToProductStockConverter(dto).Convertir();
        await productStockService.CreateManyAsync(productStocks);
        dto.Material.Status = InventoryStatus.VALIDADO;
        await materialService.ReplaceOneAsync(dto.Material.Id, dto.Material);
        return dto;
    }

    public async Task ValidarInvoiceSale(InvoiceSaleAndDetails model)
    {
        var dto = new InvoiceSaleStockDto();
        dto.InvoiceSale = model.InvoiceSale;
        dto.InvoiceSaleDetails = model.InvoiceSaleDetails;
        var productStocks = new InvoiceSaleToProductStockConverter(dto).Convertir();
        if (productStocks.Count > 0) await productStockService.CreateManyAsync(productStocks);
    }
}
