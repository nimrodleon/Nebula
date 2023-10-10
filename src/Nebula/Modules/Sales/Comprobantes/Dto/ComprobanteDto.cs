using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Helpers;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class ComprobanteDto
{
    public Company Company { get; set; } = new Company();
    #region ORIGIN_HTTP_REQUEST!
    public CabeceraComprobanteDto Cabecera { get; set; } = new CabeceraComprobanteDto();
    public List<ItemComprobanteDto> Detalle { get; set; } = new List<ItemComprobanteDto>();
    public DatoPagoComprobanteDto DatoPago { get; set; } = new DatoPagoComprobanteDto();
    public List<CuotaPagoComprobanteDto> DetallePago { get; set; } = new List<CuotaPagoComprobanteDto>();
    #endregion
    private List<SumImpItemDto> ImpItemsDto { get; set; } = new List<SumImpItemDto>();

    private SumImpVentaDto CalcularImporteVenta()
    {
        var sumImpVentaDto = new SumImpVentaDto();
        Detalle.ForEach(item =>
        {
            SumImpItemDto itemObj = new SumImpItemDto();
            decimal mtoTotalItem = item.CtdUnidadItem * item.MtoPrecioVentaUnitario;
            decimal porcentajeIGV = item.IgvSunat == TipoIGV.Gravado ? Company.PorcentajeIgv / 100 + 1 : 1;
            itemObj.MtoValorVentaItem = mtoTotalItem / porcentajeIGV;
            itemObj.MtoTriIcbperItem = item.TriIcbper ? item.CtdUnidadItem * Company.ValorImpuestoBolsa : 0;
            itemObj.MtoBaseIgvItem = mtoTotalItem / porcentajeIGV;
            itemObj.MtoIgvItem = mtoTotalItem - itemObj.MtoBaseIgvItem;
            itemObj.SumTotTributosItem = itemObj.MtoIgvItem + itemObj.MtoTriIcbperItem; // el sistema soporta solo IGV/ICBPER.
            itemObj.MtoValorUnitario = itemObj.MtoValorVentaItem / item.CtdUnidadItem;
            sumImpVentaDto.SumTotTributos += itemObj.SumTotTributosItem;
            sumImpVentaDto.SumTotValVenta += itemObj.MtoValorVentaItem;
            sumImpVentaDto.SumPrecioVenta += mtoTotalItem + itemObj.MtoTriIcbperItem;
            sumImpVentaDto.SumImpVenta = sumImpVentaDto.SumTotValVenta + sumImpVentaDto.SumTotTributos;
        });
        return sumImpVentaDto;
    }

    public InvoiceSale GetInvoiceSale()
    {
        var importeVenta = CalcularImporteVenta();
        string tipOperacion = "0101";
        return new InvoiceSale
        {
            CompanyId = Company.Id,
            DocType = Cabecera.DocType,
            TipOperacion = tipOperacion,
            FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
            HorEmision = DateTime.Now.ToString("HH:mm:ss"),
            FecVencimiento = Cabecera.FecVencimiento,
            CodLocalEmisor = Company.CodLocalEmisor,
            FormaPago = DatoPago.FormaPago,
            ContactId = Cabecera.ContactId,
            TipDocUsuario = Cabecera.TipDocUsuario,
            NumDocUsuario = Cabecera.NumDocUsuario,
            RznSocialUsuario = Cabecera.RznSocialUsuario,
            TipMoneda = Company.TipMoneda,
            SumTotValVenta = importeVenta.SumTotValVenta,
            SumPrecioVenta = importeVenta.SumPrecioVenta,
            SumTotTributos = importeVenta.SumTotTributos,
            SumImpVenta = importeVenta.SumImpVenta,
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
            Anulada = false,
            // Comentario/Observación de la factura.
            Remark = Cabecera.Remark.Trim(),
            TotalEnLetras = new NumberToLetters(importeVenta.SumImpVenta).ToString(),
        };
    }

    public List<InvoiceSaleDetail> GetInvoiceSaleDetails(string invoiceId)
    {
        var invoiceSaleDetails = new List<InvoiceSaleDetail>();
        Detalle.ForEach(item =>
        {
            // Tributo: Afectación al IGV por ítem.
            string tipAfeIgv = "10";
            string codTriIgv = string.Empty;
            string nomTributoIgvItem = string.Empty;
            string codTipTributoIgvItem = string.Empty;
            switch (item.IgvSunat)
            {
                case TipoIGV.Gravado:
                    tipAfeIgv = "10";
                    codTriIgv = "1000";
                    nomTributoIgvItem = "IGV";
                    codTipTributoIgvItem = "VAT";
                    break;
                case TipoIGV.Exonerado:
                    tipAfeIgv = "20";
                    codTriIgv = "9997";
                    nomTributoIgvItem = "EXO";
                    codTipTributoIgvItem = "VAT";
                    break;
                case TipoIGV.Inafecto:
                    tipAfeIgv = "30";
                    codTriIgv = "9998";
                    nomTributoIgvItem = "INA";
                    codTipTributoIgvItem = "FRE";
                    break;
            }
            SumImpItemDto impItemDto = new SumImpItemDto();
            impItemDto.IgvSunat = item.IgvSunat;
            decimal mtoTotalItem = item.CtdUnidadItem * item.MtoPrecioVentaUnitario;
            decimal porcentajeIGV = item.IgvSunat == TipoIGV.Gravado ? Company.PorcentajeIgv / 100 + 1 : 1;
            impItemDto.MtoValorVentaItem = mtoTotalItem / porcentajeIGV;
            impItemDto.MtoTriIcbperItem = item.TriIcbper ? item.CtdUnidadItem * Company.ValorImpuestoBolsa : 0;
            impItemDto.MtoBaseIgvItem = mtoTotalItem / porcentajeIGV;
            impItemDto.MtoIgvItem = mtoTotalItem - impItemDto.MtoBaseIgvItem;
            impItemDto.SumTotTributosItem = impItemDto.MtoIgvItem + impItemDto.MtoTriIcbperItem; // el sistema soporta solo IGV/ICBPER.
            impItemDto.MtoValorUnitario = impItemDto.MtoValorVentaItem / item.CtdUnidadItem;
            ImpItemsDto.Add(impItemDto);
            // agregar items al comprobante.
            invoiceSaleDetails.Add(new InvoiceSaleDetail
            {
                CompanyId = Company.Id,
                InvoiceSaleId = invoiceId,
                CajaDiariaId = Cabecera.CajaDiaria,
                TipoItem = item.TipoItem,
                CodUnidadMedida = item.CodUnidadMedida,
                CtdUnidadItem = item.CtdUnidadItem,
                CodProducto = item.ProductId,
                CodProductoSunat = "-", // refactorizar agregar código de barra.
                DesItem = item.DesItem,
                MtoValorUnitario = impItemDto.MtoValorUnitario,
                SumTotTributosItem = impItemDto.SumTotTributosItem,
                // Tributo: IGV(1000).
                CodTriIgv = codTriIgv,
                MtoIgvItem = impItemDto.MtoIgvItem,
                MtoBaseIgvItem = impItemDto.MtoBaseIgvItem,
                NomTributoIgvItem = nomTributoIgvItem,
                CodTipTributoIgvItem = codTipTributoIgvItem,
                TipAfeIgv = tipAfeIgv,
                PorIgvItem = item.IgvSunat == TipoIGV.Gravado ? Company.PorcentajeIgv : 0,
                // Tributo ICBPER 7152.
                CodTriIcbper = item.TriIcbper ? "7152" : "-",
                MtoTriIcbperItem = item.TriIcbper ? impItemDto.MtoTriIcbperItem : 0,
                CtdBolsasTriIcbperItem = item.TriIcbper ? Convert.ToInt32(item.CtdUnidadItem) : 0,
                NomTributoIcbperItem = "ICBPER",
                CodTipTributoIcbperItem = "OTH",
                MtoTriIcbperUnidad = item.TriIcbper ? Company.ValorImpuestoBolsa : 0,
                // Precio de Venta Unitario.
                MtoPrecioVentaUnitario = item.MtoPrecioVentaUnitario,
                MtoValorVentaItem = impItemDto.MtoValorVentaItem,
                WarehouseId = item.WarehouseId,
            });
        });
        return invoiceSaleDetails;
    }

    public List<TributoSale> GetTributoSales(string invoiceId)
    {
        decimal operaciónGravado = 0;
        decimal operaciónExonerado = 0;
        decimal operaciónInafecto = 0;
        decimal mtoTotalIgv = 0;
        decimal mtoTotalIcbper = 0;
        ImpItemsDto.ForEach(item =>
        {
            mtoTotalIcbper += item.MtoTriIcbperItem;
            switch (item.IgvSunat)
            {
                case TipoIGV.Gravado:
                    operaciónGravado += item.MtoBaseIgvItem;
                    mtoTotalIgv += item.MtoIgvItem;
                    break;
                case TipoIGV.Exonerado:
                    operaciónExonerado += item.MtoBaseIgvItem;
                    break;
                case TipoIGV.Inafecto:
                    operaciónInafecto += item.MtoBaseIgvItem;
                    break;
            }
        });
        var tributos = new List<TributoSale>();
        if (operaciónInafecto > 0)
        {
            tributos.Add(new TributoSale()
            {
                CompanyId = Company.Id,
                InvoiceSaleId = invoiceId,
                IdeTributo = "9998",
                NomTributo = "INA",
                CodTipTributo = "FRE",
                MtoBaseImponible = operaciónInafecto,
                MtoTributo = 0,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });
        }

        if (operaciónExonerado > 0)
        {
            tributos.Add(new TributoSale()
            {
                CompanyId = Company.Id,
                InvoiceSaleId = invoiceId,
                IdeTributo = "9997",
                NomTributo = "EXO",
                CodTipTributo = "VAT",
                MtoBaseImponible = operaciónExonerado,
                MtoTributo = 0,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });
        }

        if (operaciónGravado > 0)
        {
            tributos.Add(new TributoSale()
            {
                CompanyId = Company.Id,
                InvoiceSaleId = invoiceId,
                IdeTributo = "1000",
                NomTributo = "IGV",
                CodTipTributo = "VAT",
                MtoBaseImponible = operaciónGravado,
                MtoTributo = mtoTotalIgv,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });
        }

        if (mtoTotalIcbper > 0)
        {
            tributos.Add(new TributoSale()
            {
                CompanyId = Company.Id,
                InvoiceSaleId = invoiceId,
                IdeTributo = "7152",
                NomTributo = "ICBPER",
                CodTipTributo = "OTH",
                MtoBaseImponible = 0,
                MtoTributo = mtoTotalIcbper,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });
        }

        return tributos;
    }

    public List<DetallePagoSale> GetDetallePagos(string invoiceId)
    {
        var detallePagos = new List<DetallePagoSale>();
        DetallePago.ForEach(item =>
        {
            detallePagos.Add(new DetallePagoSale()
            {
                CompanyId = Company.Id,
                InvoiceSaleId = invoiceId,
                MtoCuotaPago = item.MtoCuotaPago,
                FecCuotaPago = item.FecCuotaPago,
                TipMonedaCuotaPago = Company.TipMoneda,
            });
        });
        return detallePagos;
    }

    public void GenerarSerieComprobante(ref InvoiceSerie invoiceSerie, ref InvoiceSale invoiceSale)
    {
        int numComprobante = 0;
        string THROW_MESSAGE = "Ingresa serie de comprobante!";
        switch (invoiceSale.DocType)
        {
            case "FACTURA":
                invoiceSale.Serie = invoiceSerie.Factura;
                numComprobante = invoiceSerie.CounterFactura;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante += 1;
                invoiceSerie.CounterFactura = numComprobante;
                break;
            case "BOLETA":
                invoiceSale.Serie = invoiceSerie.Boleta;
                numComprobante = invoiceSerie.CounterBoleta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante += 1;
                invoiceSerie.CounterBoleta = numComprobante;
                break;
            case "NOTA":
                invoiceSale.Serie = invoiceSerie.NotaDeVenta;
                numComprobante = invoiceSerie.CounterNotaDeVenta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante += 1;
                invoiceSerie.CounterNotaDeVenta = numComprobante;
                break;
        }

        invoiceSale.Number = numComprobante.ToString();
    }
}
