using Nebula.Modules.InvoiceHub.Dto;
using Nebula.Modules.Sales.Comprobantes.Dto;

namespace Nebula.Modules.InvoiceHub.Helpers;

public static class InvoiceMapper
{
    public static InvoiceRequestHub MapToInvoiceRequestHub(string ruc, InvoiceSaleAndDetails document)
    {
        var invoice = document.InvoiceSale;
        var details = document.InvoiceSaleDetails;
        string tipoDoc = string.Empty;
        if (invoice.DocType == "BOLETA") tipoDoc = "03";
        if (invoice.DocType == "FACTURA") tipoDoc = "01";
        var invoiceRequest = new InvoiceRequestHub()
        {
            Ruc = ruc.Trim(),
            TipoOperacion = invoice.TipOperacion,
            TipoDoc = tipoDoc,
            Serie = invoice.Serie.Trim(),
            Correlativo = invoice.Number,
            FormaPago = new FormaPagoHub()
            {
                Moneda = invoice.TipMoneda,
                Tipo = "Contado", //TODO: refactorizar.
                Monto = Math.Round(invoice.SumImpVenta, 4),
            },
            TipoMoneda = invoice.TipMoneda,
            Client = new ClientHub()
            {
                TipoDoc = invoice.TipDocUsuario.Split(":")[0].Trim(),
                NumDoc = invoice.NumDocUsuario.Trim(),
                RznSocial = invoice.RznSocialUsuario.Trim(),
            },
        };
        var detailList = new List<DetailHub>();
        details.ForEach(item =>
        {
            detailList.Add(new DetailHub()
            {
                CodProducto = item.CodProducto,
                Unidad = item.CodUnidadMedida.Split(":")[0].Trim(),
                Cantidad = Math.Round(item.CtdUnidadItem, 4),
                MtoValorUnitario = Math.Round(item.MtoValorUnitario, 4),
                Descripcion = item.DesItem.Trim(),
                MtoBaseIgv = Math.Round(item.MtoBaseIgvItem, 4),
                PorcentajeIgv = Math.Round(item.PorIgvItem, 2),
                Igv = Math.Round(item.MtoIgvItem, 4),
                TipAfeIgv = item.TipAfeIgv.Trim(),
                TotalImpuestos = Math.Round((item.MtoIgvItem + item.MtoTriIcbperItem), 4),
                MtoValorVenta = Math.Round(item.MtoValorVentaItem, 4),
                MtoPrecioUnitario = Math.Round(item.MtoPrecioVentaUnitario, 4),
            });
        });
        invoiceRequest.Details = detailList;
        return invoiceRequest;
    }
}
