using Nebula.Modules.InvoiceHub.Dto;
using Nebula.Modules.Sales.Comprobantes.Dto;

namespace Nebula.Modules.InvoiceHub.Helpers;

public class CreditNoteMapper
{
    public static CreditNoteRequestHub MapToCreditNoteRequestHub(string ruc, InvoiceCancellationResponse document)
    {
        var creditNote = document.CreditNote;
        var details = document.CreditNoteDetail;
        var creditNoteRequest = new CreditNoteRequestHub()
        {
            Ruc = ruc.Trim(),
            Serie = creditNote.Serie.Trim(),
            Correlativo = creditNote.Number,
            FechaEmision = creditNote.FecEmision,
            TipDocAfectado = creditNote.TipDocAfectado,
            NumDocAfectado = creditNote.NumDocAfectado,
            CodMotivo = creditNote.CodMotivo,
            DesMotivo = creditNote.DesMotivo,
            TipoMoneda = creditNote.TipMoneda,
            Client = new ClientHub()
            {
                TipoDoc = creditNote.TipDocUsuario.Split(":")[0].Trim(),
                NumDoc = creditNote.NumDocUsuario.Trim(),
                RznSocial = creditNote.RznSocialUsuario.Trim(),
            },
        };

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
        creditNoteRequest.Details = detailList;
        return creditNoteRequest;
    }
}
