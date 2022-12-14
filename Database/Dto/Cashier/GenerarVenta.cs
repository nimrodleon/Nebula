using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Cashier;

public class GenerarVenta
{
    /// <summary>
    /// Cabecera Comprobante.
    /// </summary>
    public Comprobante Comprobante { get; set; } = new Comprobante();

    /// <summary>
    /// Detalles del Comprobante.
    /// </summary>
    public List<DetalleComprobante> DetallesComprobante { get; set; } = new List<DetalleComprobante>();

    /// <summary>
    /// Calcular Importe Venta.
    /// </summary>
    private void CalcularImporteVenta()
    {
        Comprobante.SumTotValVenta = 0;
        Comprobante.SumTotTributos = 0;
        Comprobante.SumTotTriIcbper = 0;
        DetallesComprobante.ForEach(item =>
        {
            Comprobante.SumTotValVenta += item.MtoBaseIgvItem;
            Comprobante.SumTotTributos += item.MtoIgvItem;
            Comprobante.SumTotTriIcbper += item.MtoTriIcbperItem;
        });
        Comprobante.SumImpVenta = Comprobante.SumTotValVenta + Comprobante.SumTotTributos + Comprobante.SumTotTriIcbper;
    }

    /// <summary>
    /// Configurar cabecera de Factura.
    /// </summary>
    /// <param name="configuration">Configuración</param>
    /// <param name="contact">Contacto</param>
    /// <returns>Cabecera Factura</returns>
    public InvoiceSale GetInvoiceSale(Configuration configuration, Contact contact)
    {
        CalcularImporteVenta();
        return new InvoiceSale()
        {
            DocType = Comprobante.DocType,
            TipOperacion = "0101",
            FecEmision = DateTime.Now.ToString("yyyy-MM-dd"),
            HorEmision = DateTime.Now.ToString("HH:mm:ss"),
            CodLocalEmisor = configuration.CodLocalEmisor,
            FormaPago = Comprobante.FormaPago,
            ContactId = contact.Id,
            TipDocUsuario = contact.DocType,
            NumDocUsuario = contact.Document,
            RznSocialUsuario = contact.Name,
            TipMoneda = configuration.TipMoneda,
            SumTotValVenta = Comprobante.SumTotValVenta,
            SumTotTributos = Comprobante.SumTotTributos,
            SumImpVenta = Comprobante.SumImpVenta,
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
        };
    }

    /// <summary>
    /// Configurar Detalle de Factura.
    /// </summary>
    /// <param name="invoice">ID Cabecera</param>
    /// <param name="cajaDiaria">ID Caja diaria</param>
    /// <returns>Detalles de Factura</returns>
    public List<InvoiceSaleDetail> GetInvoiceSaleDetails(string invoice, string cajaDiaria, string warehouseId)
    {
        var invoiceSaleDetails = new List<InvoiceSaleDetail>();
        DetallesComprobante.ForEach(item =>
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

            // agregar items al comprobante.
            invoiceSaleDetails.Add(new InvoiceSaleDetail()
            {
                InvoiceSale = invoice,
                CajaDiaria = cajaDiaria,
                TipoItem = item.TipoItem,
                CodUnidadMedida = item.CodUnidadMedida,
                CtdUnidadItem = item.Quantity,
                CodProducto = item.ProductId.ToString(),
                CodProductoSunat = item.CodProductoSunat,
                DesItem = item.Description,
                MtoValorUnitario = item.MtoBaseIgvItem,
                SumTotTributosItem = item.MtoIgvItem,
                // Tributo: IGV(1000).
                CodTriIgv = codTriIgv,
                MtoIgvItem = item.MtoIgvItem,
                MtoBaseIgvItem = item.MtoBaseIgvItem,
                NomTributoIgvItem = nomTributoIgvItem,
                CodTipTributoIgvItem = codTipTributoIgvItem,
                TipAfeIgv = tipAfeIgv,
                PorIgvItem = item.IgvSunat == "EXONERADO" ? "0.00" : item.ValorIgv.ToString("N2"),
                // Tributo ICBPER 7152.
                CodTriIcbper = item.TriIcbper ? "7152" : "-",
                MtoTriIcbperItem = item.TriIcbper ? item.MtoTriIcbperItem : 0,
                CtdBolsasTriIcbperItem = item.TriIcbper ? Convert.ToInt32(item.Quantity) : 0,
                NomTributoIcbperItem = "ICBPER",
                CodTipTributoIcbperItem = "OTH",
                MtoTriIcbperUnidad = item.ValorIcbper,
                // Precio de Venta Unitario.
                MtoPrecioVentaUnitario = item.Price,
                MtoValorVentaItem = item.MtoBaseIgvItem,
                WarehouseId = warehouseId,
            });
        });
        return invoiceSaleDetails;
    }

    /// <summary>
    /// Obtener Lista de Tributos.
    /// </summary>
    /// <param name="invoice">ID Cabecera</param>
    /// <returns>Tributos de Factura</returns>
    public List<TributoSale> GetTributoSales(string invoice)
    {
        decimal opGravada = 0;
        decimal opExonerada = 0;
        decimal opGratuita = 0;
        decimal totalIgv = 0;
        decimal totalIcbper = 0;
        DetallesComprobante.ForEach(item =>
        {
            if (item.TriIcbper)
                totalIcbper += item.MtoTriIcbperItem;

            switch (item.IgvSunat)
            {
                case "GRAVADO":
                    opGravada += item.MtoBaseIgvItem;
                    totalIgv += item.MtoIgvItem;
                    break;
                case "EXONERADO":
                    opExonerada += item.MtoBaseIgvItem;
                    break;
                case "GRATUITO":
                    opGratuita += item.MtoBaseIgvItem;
                    break;
            }
        });

        var tributos = new List<TributoSale>();
        if (opGratuita > 0)
        {
            tributos.Add(new TributoSale()
            {
                InvoiceSale = invoice,
                IdeTributo = "9996",
                NomTributo = "GRA",
                CodTipTributo = "FRE",
                MtoBaseImponible = opGratuita,
                MtoTributo = 0,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });
        }

        if (opExonerada > 0)
        {
            tributos.Add(new TributoSale()
            {
                InvoiceSale = invoice,
                IdeTributo = "9997",
                NomTributo = "EXO",
                CodTipTributo = "VAT",
                MtoBaseImponible = opExonerada,
                MtoTributo = 0,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });
        }

        if (opGravada > 0)
        {
            tributos.Add(new TributoSale()
            {
                InvoiceSale = invoice,
                IdeTributo = "1000",
                NomTributo = "IGV",
                CodTipTributo = "VAT",
                MtoBaseImponible = opGravada,
                MtoTributo = totalIgv,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });
        }

        if (totalIcbper > 0)
        {
            tributos.Add(new TributoSale()
            {
                InvoiceSale = invoice,
                IdeTributo = "7152",
                NomTributo = "ICBPER",
                CodTipTributo = "OTH",
                MtoBaseImponible = 0,
                MtoTributo = totalIcbper,
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM")
            });
        }

        return tributos;
    }
}
