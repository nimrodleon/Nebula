using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Products.Helpers;
using Nebula.Modules.Sales.Invoices;

namespace Nebula.Modules.Inventory.Stock.Validations;

public interface IInvoiceSaleStockValidator
{
    Task ValidarInvoiceSale(string companyId, string id);
}

public class InvoiceSaleStockValidator : IInvoiceSaleStockValidator
{
    private readonly IProductStockService _productStockService;
    private readonly IInvoiceSaleDetailService _invoiceSaleDetailService;

    public InvoiceSaleStockValidator(IProductStockService productStockService,
        IInvoiceSaleDetailService invoiceSaleDetailService)
    {
        _productStockService = productStockService;
        _invoiceSaleDetailService = invoiceSaleDetailService;
    }

    public async Task ValidarInvoiceSale(string companyId, string id)
    {
        var invoiceSaleDetails = await _invoiceSaleDetailService.GetListAsync(companyId, id);
        var productStocks = new List<ProductStock>();
        invoiceSaleDetails.ForEach(item =>
        {
            //if (item.ControlStock == TipoControlStock.STOCK)
            //{
            //    productStocks.Add(new ProductStock()
            //    {
            //        Id = string.Empty,
            //        CompanyId = companyId.Trim(),
            //        WarehouseId = item.WarehouseId,
            //        ProductId = item.CodProducto,
            //        Type = InventoryType.SALIDA,
            //        Quantity = (long)item.CtdUnidadItem,
            //    });
            //}
        });
        if (productStocks.Count > 0)
            await _productStockService.CreateManyAsync(productStocks);
    }
}
