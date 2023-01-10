using Nebula.Database.Dto.Sales;
using Nebula.Database.Helpers;
using System.Globalization;
using System.Text.Json;

namespace Nebula.Database.Services.Facturador;

public class JsonBoletaParser
{
    public JsonBoletaParser(InvoiceSaleDto dto)
    {
        var numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
        numberFormatInfo.NumberGroupSeparator = string.Empty;
        // cabecera.
        cabecera.tipOperacion = dto.InvoiceSale.TipOperacion;
        cabecera.fecEmision = dto.InvoiceSale.FecEmision;
        cabecera.horEmision = dto.InvoiceSale.HorEmision;
        cabecera.fecVencimiento = dto.InvoiceSale.FecVencimiento;
        cabecera.codLocalEmisor = dto.InvoiceSale.CodLocalEmisor;
        cabecera.tipDocUsuario = dto.InvoiceSale.TipDocUsuario.Split(":")[0];
        cabecera.numDocUsuario = dto.InvoiceSale.NumDocUsuario;
        cabecera.rznSocialUsuario = dto.InvoiceSale.RznSocialUsuario;
        cabecera.tipMoneda = dto.InvoiceSale.TipMoneda;
        cabecera.sumTotTributos = dto.InvoiceSale.SumTotTributos.ToString("N2", numberFormatInfo);
        cabecera.sumTotValVenta = dto.InvoiceSale.SumTotValVenta.ToString("N2", numberFormatInfo);
        cabecera.sumPrecioVenta = dto.InvoiceSale.SumPrecioVenta.ToString("N2", numberFormatInfo);
        cabecera.sumImpVenta = dto.InvoiceSale.SumImpVenta.ToString("N2", numberFormatInfo);
        // adicionales de cabecera.
        if (dto.InvoiceSale.CodUbigeoCliente.Length == 6)
        {
            cabecera.adicionalCabecera = new AdicionalCabecera()
            {
                codPaisCliente = "PE",
                codUbigeoCliente = dto.InvoiceSale.CodUbigeoCliente,
                desDireccionCliente = dto.InvoiceSale.DesDireccionCliente,
            };
        }
        // detalle.
        dto.InvoiceSaleDetails.ForEach(item =>
        {
            detalle.Add(new InvoiceDetail
            {
                codUnidadMedida = item.CodUnidadMedida.Split(":")[0],
                ctdUnidadItem = item.CtdUnidadItem.ToString("N1", numberFormatInfo),
                // codProducto = item.CodProducto,
                codProducto = "-",
                codProductoSUNAT = item.CodProductoSunat,
                desItem = item.DesItem,
                mtoValorUnitario = item.MtoValorUnitario.ToString("N4", numberFormatInfo),
                sumTotTributosItem = item.SumTotTributosItem.ToString("N2", numberFormatInfo),
                // Tributo: IGV(1000).
                codTriIGV = item.CodTriIgv,
                mtoIgvItem = item.MtoIgvItem.ToString("N2", numberFormatInfo),
                mtoBaseIgvItem = item.MtoBaseIgvItem.ToString("N2", numberFormatInfo),
                nomTributoIgvItem = item.NomTributoIgvItem,
                codTipTributoIgvItem = item.CodTipTributoIgvItem,
                tipAfeIGV = item.TipAfeIgv,
                porIgvItem = item.PorIgvItem,
                // Tributo ICBPER 7152.
                codTriIcbper = item.CodTriIcbper,
                mtoTriIcbperItem = item.MtoTriIcbperItem.ToString("N2", numberFormatInfo),
                ctdBolsasTriIcbperItem = item.CtdBolsasTriIcbperItem.ToString("N0", numberFormatInfo),
                nomTributoIcbperItem = item.NomTributoIcbperItem,
                codTipTributoIcbperItem = item.CodTipTributoIcbperItem,
                mtoTriIcbperUnidad = item.MtoTriIcbperUnidad.ToString("N2", numberFormatInfo),
                // ...
                mtoPrecioVentaUnitario = item.MtoPrecioVentaUnitario.ToString("N2", numberFormatInfo),
                mtoValorVentaItem = item.MtoValorVentaItem.ToString("N2", numberFormatInfo),
                mtoValorReferencialUnitario = item.MtoValorReferencialUnitario.ToString("N4", numberFormatInfo)
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
                mtoBaseImponible = item.MtoBaseImponible.ToString("N2", numberFormatInfo),
                mtoTributo = item.MtoTributo.ToString("N2", numberFormatInfo),
            });
        });
        leyendas.Add(new Leyenda
        {
            codLeyenda = "1000",
            desLeyenda = new NumberToLetters(dto.InvoiceSale.SumImpVenta).ToString(),
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
