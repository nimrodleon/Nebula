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
            Correlativo = creditNote.Correlativo,
            FechaEmision = creditNote.FechaEmision,
            TipDocAfectado = creditNote.TipDocAfectado,
            NumDocAfectado = creditNote.NumDocfectado,
            CodMotivo = creditNote.CodMotivo,
            DesMotivo = creditNote.DesMotivo,
            TipoMoneda = creditNote.TipoMoneda,
            Client = new ClientHub()
            {
                TipoDoc = creditNote.Cliente.TipoDoc,
                NumDoc = creditNote.Cliente.NumDoc,
                RznSocial = creditNote.Cliente.RznSocial,
            },
        };

        var detailList = new List<DetailHub>();
        details.ForEach(item =>
        {
            detailList.Add(new DetailHub()
            {
                CodProducto = item.CodProducto,
                Unidad = item.Unidad.Split(":")[0].Trim(),
                Cantidad = item.Cantidad,
                MtoValorUnitario = item.MtoValorUnitario,
                Descripcion = item.Description.Trim(),
                MtoBaseIgv = item.MtoBaseIgv,
                PorcentajeIgv = Math.Round(item.PorcentajeIgv, 2),
                Igv = item.Igv,
                TipAfeIgv = item.TipAfeIgv.Trim(),
                TotalImpuestos = item.TotalImpuestos,
                MtoValorVenta = item.MtoValorVenta,
                MtoPrecioUnitario = item.MtoPrecioUnitario,
            });
        });
        creditNoteRequest.Details = detailList;
        return creditNoteRequest;
    }
}
