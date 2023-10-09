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
            FechaEmision = invoice.FecEmision,
            FormaPago = new FormaPagoHub()
            {
                Moneda = invoice.TipMoneda,
                Tipo = invoice.FormaPago.Split(":")[0].Trim(),
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
        if (invoiceRequest.FormaPago.Tipo == "Credito" && invoice.DocType == "FACTURA")
        {
            invoiceRequest.FecVencimiento = invoice.FecVencimiento;
            document.DetallePagoSale.ForEach(item =>
            {
                invoiceRequest.Cuotas.Add(new CuotaHub
                {
                    Moneda = item.TipMonedaCuotaPago.Trim(),
                    Monto = item.MtoCuotaPago,
                    FechaPago = item.FecCuotaPago,
                });
            });
        }
        var detailList = new List<DetailHub>();
        details.ForEach(item =>
        {
            detailList.Add(new DetailHub()
            {
                CodProducto = item.CodProducto,
                Unidad = item.CodUnidadMedida.Split(":")[0].Trim(),
                Cantidad = item.CtdUnidadItem,
                MtoValorUnitario = item.MtoValorUnitario,
                Descripcion = item.DesItem.Trim(),
                MtoBaseIgv = item.MtoBaseIgvItem,
                PorcentajeIgv = Math.Round(item.PorIgvItem, 2),
                Igv = item.MtoIgvItem,
                TipAfeIgv = item.TipAfeIgv.Trim(),
                Icbper = item.MtoTriIcbperItem,
                FactorIcbper = item.MtoTriIcbperUnidad,
                TotalImpuestos = item.MtoIgvItem + item.MtoTriIcbperItem,
                MtoValorVenta = item.MtoValorVentaItem,
                MtoPrecioUnitario = item.MtoPrecioVentaUnitario,
            });
        });
        invoiceRequest.Details = detailList;
        return invoiceRequest;
    }
}
