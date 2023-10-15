using Nebula.Modules.InvoiceHub.Dto;
using Nebula.Modules.Sales.Comprobantes.Dto;

namespace Nebula.Modules.InvoiceHub.Helpers;

public static class InvoiceMapper
{
    public static InvoiceRequestHub MapToInvoiceRequestHub(string ruc, InvoiceSaleAndDetails document)
    {
        var invoice = document.InvoiceSale;
        var details = document.InvoiceSaleDetails;
        var invoiceRequest = new InvoiceRequestHub()
        {
            Ruc = ruc.Trim(),
            TipoOperacion = invoice.TipoOperacion,
            TipoDoc = invoice.TipoDoc,
            Serie = invoice.Serie.Trim(),
            Correlativo = invoice.Correlativo,
            FechaEmision = invoice.FechaEmision,
            FormaPago = new FormaPagoHub()
            {
                Moneda = invoice.FormaPago.Moneda,
                Tipo = invoice.FormaPago.Tipo,
                Monto = invoice.FormaPago.Monto,
            },
            TipoMoneda = invoice.TipoMoneda,
            Client = new ClientHub()
            {
                TipoDoc = invoice.Cliente.TipoDoc,
                NumDoc = invoice.Cliente.NumDoc,
                RznSocial = invoice.Cliente.RznSocial,
            },
        };
        if (invoiceRequest.FormaPago.Tipo == "Credito" && invoice.TipoDoc == "01")
        {
            invoiceRequest.FecVencimiento = invoice.FecVencimiento;
            //document.DetallePagoSale.ForEach(item =>
            //{
            //    invoiceRequest.Cuotas.Add(new CuotaHub
            //    {
            //        Moneda = item.TipMonedaCuotaPago.Trim(),
            //        Monto = item.MtoCuotaPago,
            //        FechaPago = item.FecCuotaPago,
            //    });
            //});
        }
        var detailList = new List<DetailHub>();
        details.ForEach(item =>
        {
            detailList.Add(new DetailHub()
            {
                CodProducto = item.CodProducto,
                Unidad = item.Unidad.Split(":")[0].Trim(),
                Cantidad = item.Cantidad,
                MtoValorUnitario = item.MtoValorUnitario,
                Descripcion = item.Description,
                MtoBaseIgv = item.MtoBaseIgv,
                PorcentajeIgv = Math.Round(item.PorcentajeIgv, 2),
                Igv = item.Igv,
                TipAfeIgv = item.TipAfeIgv.Trim(),
                TotalImpuestos = item.TotalImpuestos,
                MtoValorVenta = item.MtoValorVenta,
                MtoPrecioUnitario = item.MtoPrecioUnitario,
            });
        });
        invoiceRequest.Details = detailList;
        return invoiceRequest;
    }
}
