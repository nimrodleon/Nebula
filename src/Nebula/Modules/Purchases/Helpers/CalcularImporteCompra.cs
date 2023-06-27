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
        _purchaseInvoiceDetailList.ForEach(item =>
        {
            decimal mtoTotalItem = item.CtdUnidadItem * item.MtoPrecioCompraUnitario;
            purchaseInvoice.SumTotTributos += item.SumTotTributosItem;
            purchaseInvoice.SumTotValCompra += item.MtoValorCompraItem;
            purchaseInvoice.SumPrecioCompra += mtoTotalItem + item.MtoTriIcbperItem;
            purchaseInvoice.SumImpCompra = purchaseInvoice.SumTotValCompra + purchaseInvoice.SumTotTributos;
        });
        return purchaseInvoice;
    }

}
