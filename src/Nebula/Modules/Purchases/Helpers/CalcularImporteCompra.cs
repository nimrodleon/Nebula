using Nebula.Modules.Purchases.Models;

namespace Nebula.Modules.Purchases.Helpers;

public class CalcularImporteCompra
{
    private readonly List<PurchaseInvoiceDetail> _purchaseInvoiceDetailList;

    public CalcularImporteCompra(List<PurchaseInvoiceDetail> purchaseInvoiceDetailList)
    {
        _purchaseInvoiceDetailList = purchaseInvoiceDetailList;
    }

    public PurchaseInvoice Calcular(PurchaseInvoice purchaseInvoice)
    {
        purchaseInvoice.SumTotTributos = 0;
        purchaseInvoice.SumTotValCompra = 0;
        purchaseInvoice.SumPrecioCompra = 0;
        purchaseInvoice.SumImpCompra = 0;
        // Sumatoria impuestos igv y icbper.
        purchaseInvoice.SumTotMtoIgv = 0;
        purchaseInvoice.SumTotMtoTriIcbper = 0;
        _purchaseInvoiceDetailList.ForEach(item =>
        {
            decimal mtoTotalItem = item.CtdUnidadItem * item.MtoPrecioCompraUnitario;
            purchaseInvoice.SumTotTributos += item.SumTotTributosItem;
            purchaseInvoice.SumTotValCompra += item.MtoValorCompraItem;
            purchaseInvoice.SumPrecioCompra += mtoTotalItem + item.MtoTriIcbperItem;
            purchaseInvoice.SumImpCompra = purchaseInvoice.SumTotValCompra + purchaseInvoice.SumTotTributos;
            // Sumatoria impuestos igv y icbper.
            purchaseInvoice.SumTotMtoIgv += item.MtoIgvItem;
            purchaseInvoice.SumTotMtoTriIcbper += item.MtoTriIcbperItem;
        });
        return purchaseInvoice;
    }

}
