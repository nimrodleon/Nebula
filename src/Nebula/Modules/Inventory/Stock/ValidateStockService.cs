using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Dto;
using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Notas;
using Nebula.Modules.Inventory.Stock.Converter;
using Nebula.Modules.Inventory.Stock.Helpers;
using Nebula.Modules.Inventory.Transferencias;
using Nebula.Modules.Sales.Invoices;

namespace Nebula.Modules.Inventory.Stock;

public interface IValidateStockService
{
    Task<InventoryNoteDto> ValidarNotas(string companyId, string InventoryNotasId);
    Task<TransferenciaDto> ValidarTransferencia(string companyId, string transferenciaId);
    Task<AjusteInventarioDto> ValidarAjusteInventario(string companyId, string ajusteInventarioId);
    Task<MaterialDto> ValidarMaterial(string companyId, string materialId);
    Task ValidarInvoiceSale(string companyId, string invoiceSaleId);
}

public class ValidateStockService : IValidateStockService
{
    private readonly IProductStockService _productStockService;
    private readonly IInventoryNotasService _inventoryNotasService;
    private readonly IInventoryNotasDetailService _inventoryNotasDetailService;
    private readonly ITransferenciaService _transferenciaService;
    private readonly ITransferenciaDetailService _transferenciaDetailService;
    private readonly IAjusteInventarioService _ajusteInventarioService;
    private readonly IAjusteInventarioDetailService _ajusteInventarioDetailService;
    private readonly IMaterialService _materialService;
    private readonly IMaterialDetailService _materialDetailService;
    private readonly IInvoiceSaleService _invoiceSaleService;
    private readonly IInvoiceSaleDetailService _invoiceDetailService;

    public ValidateStockService(IProductStockService productStockService,
        IInventoryNotasService inventoryNotasService, IInventoryNotasDetailService inventoryNotasDetailService,
        ITransferenciaService transferenciaService, ITransferenciaDetailService transferenciaDetailService,
        IAjusteInventarioService ajusteInventarioService, IAjusteInventarioDetailService ajusteInventarioDetailService,
        IMaterialService materialService, IMaterialDetailService materialDetailService,
        IInvoiceSaleService invoiceSaleService, IInvoiceSaleDetailService invoiceSaleDetailService)
    {
        _productStockService = productStockService;
        _inventoryNotasService = inventoryNotasService;
        _inventoryNotasDetailService = inventoryNotasDetailService;
        _transferenciaService = transferenciaService;
        _transferenciaDetailService = transferenciaDetailService;
        _ajusteInventarioService = ajusteInventarioService;
        _ajusteInventarioDetailService = ajusteInventarioDetailService;
        _materialService = materialService;
        _materialDetailService = materialDetailService;
        _invoiceSaleService = invoiceSaleService;
        _invoiceDetailService = invoiceSaleDetailService;
    }

    public async Task<InventoryNoteDto> ValidarNotas(string companyId, string InventoryNotasId)
    {
        var dto = new InventoryNoteDto();
        dto.InventoryNotas = await _inventoryNotasService.GetByIdAsync(companyId, InventoryNotasId);
        dto.InventoryNotasDetail = await _inventoryNotasDetailService.GetListAsync(companyId, dto.InventoryNotas.Id);
        var productStocks = new InventoryNoteToProductStockConverter(dto).Convertir();
        await _productStockService.CreateManyAsync(productStocks);
        dto.InventoryNotas.Status = InventoryStatus.VALIDADO;
        await _inventoryNotasService.UpdateAsync(dto.InventoryNotas.Id, dto.InventoryNotas);
        return dto;
    }

    public async Task<TransferenciaDto> ValidarTransferencia(string companyId, string transferenciaId)
    {
        var dto = new TransferenciaDto();
        dto.Transferencia = await _transferenciaService.GetByIdAsync(companyId, transferenciaId);
        dto.TransferenciaDetails = await _transferenciaDetailService.GetListAsync(companyId, dto.Transferencia.Id);

        var productArrId = new List<string>();
        dto.TransferenciaDetails.ForEach(item => productArrId.Add(item.ProductId));
        var productStocks = await _productStockService.GetProductStockByProductIdsAsync(companyId, dto.Transferencia.WarehouseOriginId, productArrId);
        dto.TransferenciaDetails = new TransferenciaCalculator(productStocks, dto.TransferenciaDetails).CalcularCantidadesRestante(companyId);

        var result = new TransferenciaToProductStockConverter(dto).Convertir();
        await _productStockService.CreateManyAsync(result.ProductStockOrigen);
        await _productStockService.CreateManyAsync(result.ProductStockDestino);

        await _transferenciaDetailService.DeleteManyAsync(companyId, dto.Transferencia.Id);
        await _transferenciaDetailService.InsertManyAsync(dto.TransferenciaDetails);

        dto.Transferencia.Status = InventoryStatus.VALIDADO;
        await _transferenciaService.UpdateAsync(dto.Transferencia.Id, dto.Transferencia);
        return dto;
    }

    public async Task<AjusteInventarioDto> ValidarAjusteInventario(string companyId, string ajusteInventarioId)
    {
        var dto = new AjusteInventarioDto();
        dto.AjusteInventario = await _ajusteInventarioService.GetByIdAsync(companyId, ajusteInventarioId);
        dto.AjusteInventarioDetails = await _ajusteInventarioDetailService.GetListAsync(companyId, dto.AjusteInventario.Id);

        var productArrId = new List<string>();
        dto.AjusteInventarioDetails.ForEach(item => productArrId.Add(item.ProductId));
        var productStocks = await _productStockService.GetProductStockByProductIdsAsync(companyId, dto.AjusteInventario.WarehouseId, productArrId);
        dto.AjusteInventarioDetails = new AjusteInventarioCalculator(productStocks, dto.AjusteInventarioDetails).CalcularCantidadExistente(companyId);
        await _productStockService.DeleteProductStockByProductIdsAsync(companyId, dto.AjusteInventario.WarehouseId, productArrId);

        var newProductStocks = new AjusteInventarioToProductStockConverter(dto).Convertir();
        await _productStockService.CreateManyAsync(newProductStocks);
        await _ajusteInventarioDetailService.DeleteManyAsync(companyId, dto.AjusteInventario.Id);
        await _ajusteInventarioDetailService.InsertManyAsync(dto.AjusteInventarioDetails);

        dto.AjusteInventario.Status = InventoryStatus.VALIDADO;
        await _ajusteInventarioService.UpdateAsync(dto.AjusteInventario.Id, dto.AjusteInventario);
        return dto;
    }

    public async Task<MaterialDto> ValidarMaterial(string companyId, string materialId)
    {
        var dto = new MaterialDto();
        dto.Material = await _materialService.GetByIdAsync(companyId, materialId);
        dto.MaterialDetails = await _materialDetailService.GetListAsync(companyId, dto.Material.Id);
        var productStocks = new MaterialToProductStockConverter(dto).Convertir();
        await _productStockService.CreateManyAsync(productStocks);
        dto.Material.Status = InventoryStatus.VALIDADO;
        await _materialService.UpdateAsync(dto.Material.Id, dto.Material);
        return dto;
    }

    public async Task ValidarInvoiceSale(string companyId, string invoiceSaleId)
    {
        var dto = new InvoiceSaleStockDto();
        dto.InvoiceSale = await _invoiceSaleService.GetByIdAsync(companyId, invoiceSaleId);
        dto.InvoiceSaleDetails = await _invoiceDetailService.GetListAsync(companyId, dto.InvoiceSale.Id);
        var productStocks = new InvoiceSaleToProductStockConverter(dto).Convertir();
        await _productStockService.CreateManyAsync(productStocks);
    }
}
