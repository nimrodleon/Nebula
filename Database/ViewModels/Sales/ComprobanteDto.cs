using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.ViewModels.Sales;

public class ComprobanteDto
{
    struct Item
    {
        /// <summary>
        /// Valor Unitario sin Ningún Tributo.
        /// </summary>
        public decimal MtoValorUnitario { get; set; }
        /// <summary>
        /// Sumatorio de Todos los Tributos sin ICBPER.
        /// </summary>
        public decimal SumTotTributosItem { get; set; }
        /// <summary>
        /// Monto de IGV por ítem.
        /// </summary>
        public decimal MtoIgvItem { get; set; }
        /// <summary>
        /// Base Imponible IGV por Item.
        /// Siempre mayor a cero. Si el item tiene ISC, agregar dicho monto a la base imponible.
        /// </summary>
        public decimal MtoBaseIgvItem { get; set; }
        /// <summary>
        /// Monto de tributo ICBPER por iItem.
        /// </summary>
        public decimal MtoTriIcbperItem { get; set; }
        /// <summary>
        /// Total Venta Total Item sin Ningún Tributo.
        /// </summary>
        public decimal MtoValorVentaItem { get; set; }
    }
    struct Total
    {
        public decimal SumTotValVenta { get; set; }
        public decimal SumTotTributos { get; set; }
        public decimal SumImpVenta { get; set; }
    }
    private Configuration _configuration = new Configuration();
    #region ORIGIN_HTTP_REQUEST!
    public CabeceraComprobanteDto Cabecera { get; set; } = new CabeceraComprobanteDto();
    public List<ItemComprobanteDto> Detalle { get; set; } = new List<ItemComprobanteDto>();
    public DatoPagoComprobanteDto DatoPago { get; set; } = new DatoPagoComprobanteDto();
    public List<CuotaPagoComprobanteDto> DetallePago { get; set; } = new List<CuotaPagoComprobanteDto>();
    #endregion
    public void SetConfiguration(Configuration configuration)
    {
        _configuration = configuration;
    }

    public InvoiceSale GetInvoiceSale()
    {
        return new InvoiceSale
        {
            DocType = Cabecera.DocType,
            TipOperacion = "0101",
            FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
            HorEmision = DateTime.Now.ToString("HH:mm:ss"),
            FecVencimiento = Cabecera.FecVencimiento,
            CodLocalEmisor = _configuration.CodLocalEmisor,
            FormaPago = DatoPago.FormaPago,
            ContactId = Cabecera.ContactId,
            TipDocUsuario = Cabecera.TipDocUsuario,
            NumDocUsuario = Cabecera.NumDocUsuario,
            RznSocialUsuario = Cabecera.RznSocialUsuario,
            TipMoneda = _configuration.TipMoneda,
            // ... agregar sub totales.
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
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
                case "GRAVADO":
                    tipAfeIgv = "10";
                    codTriIgv = "1000";
                    nomTributoIgvItem = "IGV";
                    codTipTributoIgvItem = "VAT";
                    break;
                case "EXONERADO":
                    tipAfeIgv = "20";
                    codTriIgv = "9997";
                    nomTributoIgvItem = "EXO";
                    codTipTributoIgvItem = "VAT";
                    break;
                case "GRATUITO":
                    tipAfeIgv = "21";
                    codTriIgv = "9996";
                    nomTributoIgvItem = "GRA";
                    codTipTributoIgvItem = "FRE";
                    break;
            }
            Item itemObj = new Item();
            decimal porcentajeIGV = item.IgvSunat == "GRAVADO" ? (_configuration.PorcentajeIgv / 100) + 1 : 1;
            itemObj.MtoValorVentaItem = item.ImporteTotalItem / porcentajeIGV;
            itemObj.MtoTriIcbperItem = item.CtdUnidadItem * _configuration.ValorImpuestoBolsa;

            // agregar items al comprobante.
            invoiceSaleDetails.Add(new InvoiceSaleDetail
            {
                InvoiceSale = invoiceId,
                CajaDiaria = "-",
                TipoItem = item.TipoItem,
                CodUnidadMedida = item.CodUnidadMedida,
                CtdUnidadItem = item.CtdUnidadItem,
                CodProducto = item.ProductId,
                CodProductoSunat = "-",
                DesItem = item.DesItem,
                // MtoValorUnitario =
                // SumTotTributosItem =
                // ---
                // Tributo: IGV(1000).
                CodTriIgv = codTriIgv,
                // MtoIgvItem =
                // MtoBaseIgvItem =
                NomTributoIgvItem = nomTributoIgvItem,
                CodTipTributoIgvItem = codTipTributoIgvItem,
                TipAfeIgv = tipAfeIgv,
                PorIgvItem = item.IgvSunat == "EXONERADO" ? "0.00" : _configuration.PorcentajeIgv.ToString("N2"),
                // Tributo ICBPER 7152.
                CodTriIcbper = item.TriIcbper ? "7152" : "-",
                MtoTriIcbperItem = itemObj.MtoTriIcbperItem,
                CtdBolsasTriIcbperItem = item.TriIcbper ? Convert.ToInt32(item.CtdUnidadItem) : 0,
                NomTributoIcbperItem = "ICBPER",
                CodTipTributoIcbperItem = "OTH",
                MtoTriIcbperUnidad = item.TriIcbper ? _configuration.ValorImpuestoBolsa : 0,
                // Precio de Venta Unitario.
                MtoPrecioVentaUnitario = item.SalidaInventario == "SI" ? item.MtoPrecioVentaUnitario : 0,
                MtoValorVentaItem = itemObj.MtoValorVentaItem,
                WarehouseId = item.WarehouseId,
            });
        });
        return invoiceSaleDetails;
    }

}
