using Nebula.Modules.Facturador.Models;
using Nebula.Modules.Sales.Dto;
using Nebula.Modules.Sales.Helpers;
using System.Globalization;
using System.Text.Json;

namespace Nebula.Modules.Facturador.JsonParser;

public class JsonFacturaParser
{
    public JsonFacturaParser(InvoiceSaleDto dto)
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
        cabecera.numDocUsuario = dto.InvoiceSale.NumDocUsuario.Trim();
        cabecera.rznSocialUsuario = dto.InvoiceSale.RznSocialUsuario.Trim();
        cabecera.tipMoneda = dto.InvoiceSale.TipMoneda;
        cabecera.sumTotTributos = dto.InvoiceSale.SumTotTributos.ToString("N2", numberFormatInfo);
        cabecera.sumTotValVenta = dto.InvoiceSale.SumTotValVenta.ToString("N2", numberFormatInfo);
        cabecera.sumPrecioVenta = dto.InvoiceSale.SumPrecioVenta.ToString("N2", numberFormatInfo);
        cabecera.sumImpVenta = dto.InvoiceSale.SumImpVenta.ToString("N2", numberFormatInfo);
        // adicionales de cabecera.
        if (dto.InvoiceSale.CodUbigeoCliente.Trim().Length == 6)
        {
            cabecera.adicionalCabecera = new FacturadorAdicionalCabecera()
            {
                codPaisCliente = "PE",
                codUbigeoCliente = dto.InvoiceSale.CodUbigeoCliente.Trim(),
                desDireccionCliente = dto.InvoiceSale.DesDireccionCliente.Trim(),
            };
        }

        // detalle.
        dto.InvoiceSaleDetails.ForEach(item =>
        {
            detalle.Add(new FacturadorInvoiceDetail
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
        dto.TributoSales.ForEach(item =>
        {
            tributos.Add(new FacturadorTributo
            {
                ideTributo = item.IdeTributo,
                nomTributo = item.NomTributo,
                codTipTributo = item.CodTipTributo,
                mtoBaseImponible = item.MtoBaseImponible.ToString("N2", numberFormatInfo),
                mtoTributo = item.MtoTributo.ToString("N2", numberFormatInfo),
            });
        });
        leyendas.Add(new FacturadorLeyenda
        {
            codLeyenda = "1000",
            desLeyenda = new NumberToLetters(dto.InvoiceSale.SumImpVenta).ToString(),
        });
        // configurar forma de pago.
        string formaPago = dto.InvoiceSale.FormaPago.Split(":")[0];
        if (formaPago == "Contado")
        {
            datoPago.formaPago = "Contado";
            datoPago.mtoNetoPendientePago = dto.InvoiceSale.SumImpVenta.ToString("N2", numberFormatInfo);
            datoPago.tipMonedaMtoNetoPendientePago = dto.InvoiceSale.TipMoneda;
        }

        if (formaPago == "Credito")
        {
            datoPago.formaPago = "Credito";
            datoPago.mtoNetoPendientePago = dto.InvoiceSale.SumImpVenta.ToString("N2", numberFormatInfo);
            datoPago.tipMonedaMtoNetoPendientePago = dto.InvoiceSale.TipMoneda;
            dto.DetallePagoSales.ForEach(item =>
            {
                detallePago.Add(new FacturadorItemFormaPago()
                {
                    mtoCuotaPago = item.MtoCuotaPago.ToString("N2", numberFormatInfo),
                    fecCuotaPago = item.FecCuotaPago,
                    tipMonedaCuotaPago = item.TipMonedaCuotaPago,
                });
            });
        }
    }

    public FacturadorInvoice cabecera { get; set; } = new FacturadorInvoice();
    public List<FacturadorInvoiceDetail> detalle { get; set; } = new List<FacturadorInvoiceDetail>();
    public List<FacturadorTributo> tributos { get; set; } = new List<FacturadorTributo>();
    public List<FacturadorLeyenda> leyendas { get; set; } = new List<FacturadorLeyenda>();
    public FacturadorFormaPago datoPago { get; set; } = new FacturadorFormaPago();
    public List<FacturadorItemFormaPago> detallePago { get; set; } = new List<FacturadorItemFormaPago>();

    public void CreateJson(string path)
    {
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText(path, jsonString);
    }
}
