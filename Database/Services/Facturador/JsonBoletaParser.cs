using Nebula.Database.Helpers;
using System.Text.Json;

namespace Nebula.Database.Services.Facturador;

public class JsonBoletaParser
{
    public JsonBoletaParser(InvoiceSaleDto dto)
    {
        // cabecera.
        cabecera.tipOperacion = dto.InvoiceSale.TipOperacion;
        cabecera.fecEmision = dto.InvoiceSale.FecEmision;
        cabecera.horEmision = dto.InvoiceSale.HorEmision;
        cabecera.fecVencimiento = dto.InvoiceSale.FecVencimiento;
        cabecera.codLocalEmisor = dto.InvoiceSale.CodLocalEmisor;
        cabecera.tipDocUsuario = dto.InvoiceSale.TipDocUsuario;
        cabecera.numDocUsuario = dto.InvoiceSale.NumDocUsuario;
        cabecera.rznSocialUsuario = dto.InvoiceSale.RznSocialUsuario;
        cabecera.tipMoneda = dto.InvoiceSale.TipMoneda;
        cabecera.sumTotTributos = dto.InvoiceSale.SumTotTributos.ToString("N2");
        cabecera.sumTotValVenta = dto.InvoiceSale.SumTotValVenta.ToString("N2");
        cabecera.sumPrecioVenta = dto.InvoiceSale.SumPrecioVenta.ToString("N2");
        cabecera.sumImpVenta = dto.InvoiceSale.SumImpVenta.ToString("N2");
        // detalle.
        dto.InvoiceSaleDetails.ForEach(item =>
        {
            detalle.Add(new InvoiceDetail
            {
                codUnidadMedida = item.CodUnidadMedida.Split(":")[0],
                ctdUnidadItem = item.CtdUnidadItem.ToString("N2"),
                codProducto = item.CodProducto,
                codProductoSUNAT = item.CodProductoSunat,
                desItem = item.DesItem,
                mtoValorUnitario = item.MtoValorUnitario.ToString("N2"),
                sumTotTributosItem = item.SumTotTributosItem.ToString("N2"),
                // Tributo: IGV(1000).
                codTriIGV = item.CodTriIgv,
                mtoIgvItem = item.MtoIgvItem.ToString("N2"),
                mtoBaseIgvItem = item.MtoBaseIgvItem.ToString("N2"),
                nomTributoIgvItem = item.NomTributoIgvItem,
                codTipTributoIgvItem = item.CodTipTributoIgvItem,
                tipAfeIGV = item.TipAfeIgv,
                porIgvItem = item.PorIgvItem,
                // Tributo ICBPER 7152.
                codTriIcbper = item.CodTriIcbper,
                mtoTriIcbperItem = item.MtoTriIcbperItem.ToString("N2"),
                ctdBolsasTriIcbperItem = item.CtdBolsasTriIcbperItem.ToString("N0"),
                nomTributoIcbperItem = item.NomTributoIcbperItem,
                codTipTributoIcbperItem = item.CodTipTributoIcbperItem,
                mtoTriIcbperUnidad = item.MtoTriIcbperUnidad.ToString("N2"),
                // ...
                mtoPrecioVentaUnitario = item.MtoPrecioVentaUnitario.ToString("N2"),
                mtoValorVentaItem = item.MtoValorVentaItem.ToString("N2"),
                mtoValorReferencialUnitario = item.MtoValorReferencialUnitario.ToString("N2")
            });
        });
        // tributos.
        dto.TributoSales.ForEach(item =>
        {
            tributos.Add(new Tributo
            {
                ideTributo = item.IdeTributo,
                nomTributo = item.NomTributo,
                codTipTributo = item.CodTipTributo,
                mtoBaseImponible = item.MtoBaseImponible.ToString("N2"),
                mtoTributo = item.MtoTributo.ToString("N2"),
            });
            var montoTotal = item.MtoBaseImponible + item.MtoTributo;
            leyendas.Add(new Leyenda
            {
                codLeyenda = item.IdeTributo,
                desLeyenda = new NumberToLetters(montoTotal).ToString()
            });
        });
    }

    public Invoice cabecera { get; set; } = new Invoice();
    public List<InvoiceDetail> detalle { get; set; } = new List<InvoiceDetail>();
    public List<Tributo> tributos { get; set; } = new List<Tributo>();
    public List<Leyenda> leyendas { get; set; } = new List<Leyenda>();

    public void CreateJson(string path)
    {
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText(path, jsonString);
    }
}
