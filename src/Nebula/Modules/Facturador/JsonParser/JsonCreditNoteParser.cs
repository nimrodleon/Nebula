using Nebula.Modules.Facturador.Models;
using Nebula.Modules.Sales.Dto;
using Nebula.Modules.Sales.Helpers;
using System.Globalization;
using System.Text.Json;

namespace Nebula.Modules.Facturador.JsonParser;

public class JsonCreditNoteParser
{
    public JsonCreditNoteParser(CreditNoteDto dto)
    {
        var numberFormatInfo = new CultureInfo("en-US", false).NumberFormat;
        numberFormatInfo.NumberGroupSeparator = string.Empty;
        // cabecera.
        cabecera.tipOperacion = dto.CreditNote.TipOperacion;
        cabecera.fecEmision = dto.CreditNote.FecEmision;
        cabecera.horEmision = dto.CreditNote.HorEmision;
        cabecera.codLocalEmisor = dto.CreditNote.CodLocalEmisor;
        cabecera.tipDocUsuario = dto.CreditNote.TipDocUsuario.Split(":")[0];
        cabecera.numDocUsuario = dto.CreditNote.NumDocUsuario.Trim();
        cabecera.rznSocialUsuario = dto.CreditNote.RznSocialUsuario.Trim();
        cabecera.tipMoneda = dto.CreditNote.TipMoneda;
        cabecera.codMotivo = dto.CreditNote.CodMotivo;
        cabecera.desMotivo = dto.CreditNote.DesMotivo;
        cabecera.tipDocAfectado = dto.CreditNote.TipDocAfectado;
        cabecera.numDocAfectado = dto.CreditNote.NumDocAfectado;
        cabecera.sumTotTributos = dto.CreditNote.SumTotTributos.ToString("N2", numberFormatInfo);
        cabecera.sumTotValVenta = dto.CreditNote.SumTotValVenta.ToString("N2", numberFormatInfo);
        cabecera.sumPrecioVenta = dto.CreditNote.SumPrecioVenta.ToString("N2", numberFormatInfo);
        cabecera.sumImpVenta = dto.CreditNote.SumImpVenta.ToString("N2", numberFormatInfo);
        // adicionales de cabecera.
        if (dto.CreditNote.CodUbigeoCliente.Trim().Length == 6)
        {
            cabecera.adicionalCabecera = new AdicionalCabecera()
            {
                codPaisCliente = "PE",
                codUbigeoCliente = dto.CreditNote.CodUbigeoCliente.Trim(),
                desDireccionCliente = dto.CreditNote.DesDireccionCliente.Trim(),
            };
        }

        // detalle.
        dto.CreditNoteDetails.ForEach(item =>
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
                porIgvItem = item.PorIgvItem.ToString("N2", numberFormatInfo),
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
        dto.TributosCreditNote.ForEach(item =>
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
            desLeyenda = new NumberToLetters(dto.CreditNote.SumImpVenta).ToString(),
        });
    }

    public CreditNoteFact cabecera { get; set; } = new CreditNoteFact();
    public List<InvoiceDetail> detalle { get; set; } = new List<InvoiceDetail>();
    public List<Tributo> tributos { get; set; } = new List<Tributo>();
    public List<Leyenda> leyendas { get; set; } = new List<Leyenda>();

    public void CreateJson(string path)
    {
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText(path, jsonString);
    }
}
