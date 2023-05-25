using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Products.Helpers;
using Nebula.Modules.Sales.Comprobantes.Dto;
using Nebula.Modules.Sales.Helpers;

namespace Testings.Modules.Sales.Dto;

[TestClass]
public class BoletaComprobanteDtoTests
{
    private ComprobanteDto _comprobanteDto = new ComprobanteDto();
    private Configuration _configuration = new Configuration();

    [TestInitialize]
    public void Initialize()
    {
        _comprobanteDto = new ComprobanteDto()
        {
            Cabecera = new CabeceraComprobanteDto()
            {
                CajaDiaria = "646408b44f3c2e66571e17fc",
                DocType = "BOLETA",
                ContactId = "625e2411f2282d217b06449f",
                TipDocUsuario = "1",
                NumDocUsuario = "12345678",
                RznSocialUsuario = "CLIENTE A",
                InvoiceSerieId = "630461a24947a22864ef9652",
                Remark = "-",
                CodUbigeoCliente = "030306",
                DesDireccionCliente = "JR. GUILLERMO CACERES TRESIERRA NRO. 178",
            },
            Detalle = new List<ItemComprobanteDto>() {
                new ItemComprobanteDto() {
                    TipoItem = "BIEN",
                    CtdUnidadItem = 1,
                    CodUnidadMedida = "NIU:UNIDAD (BIENES)",
                    DesItem = "MONITOR DELL 21 PULG, NEGRO",
                    TriIcbper = false,
                    IgvSunat = TipoIGV.Gravado,
                    ProductId = "625ecd541f6bf6e6b66a2683",
                    MtoPrecioVentaUnitario = 480,
                    WarehouseId = "625e24bff2282d217b0644a1",
                    ControlStock = TipoControlStock.STOCK,
                },
                new ItemComprobanteDto() {
                    TipoItem = "BIEN",
                    CtdUnidadItem = 1,
                    CodUnidadMedida = "NIU:UNIDAD (BIENES)",
                    DesItem = "TECLADO MULTIMEDIA CYBERTEL",
                    TriIcbper = false,
                    IgvSunat = TipoIGV.Gravado,
                    ProductId = "625ecda31f6bf6e6b66a2684",
                    MtoPrecioVentaUnitario = 20,
                    WarehouseId = "625e24bff2282d217b0644a1",
                    ControlStock = TipoControlStock.STOCK,
                }
            },
            DatoPago = new DatoPagoComprobanteDto()
            {
                FormaPago = "Contado",
                MtoNetoPendientePago = 500
            },
            DetallePago = new List<CuotaPagoComprobanteDto>()
            {

            }
        };
        _configuration = new Configuration()
        {
            Id = "DEFAULT",
            Ruc = "20527175829",
            RznSocial = "DIRECOM S.R.L.",
            Address = "JR. GUILLERMO CACERES TRESIERRA NRO. 178 RES. CERCADO (A 1 CDRA DE LA CURACAO)",
            PhoneNumber = "983993363",
            AnchoTicket = "410",
            CodLocalEmisor = "0000",
            TipMoneda = "PEN",
            PorcentajeIgv = 18,
            ValorImpuestoBolsa = 0.5M,
            CpeSunat = "SFS",
            ModoEnvioSunat = "ENVIAR",
            ContactId = "625e2411f2282d217b06449f",
            DiasPlazo = 30,
            AccessToken = "hw8H3F9/OS0IJbDa3YskdDYWCPgt1pJ0M56wtvgRziTUmD56fDfso3MBJlwDcFftKix7EGplQuEXBA7TZG9fkoYmm2Yp3scCXmLcl9hCNPtNoafGGYCLZeX6IqjelHQ6jv3mMHsgtw5pdZnsi0SPRH5OEkaWCiRZWWPsC6sscK8=",
            SubscriptionId = "94595260-8262-453e-876a-7c7429464e0f",
            FacturadorUrl = "http://localhost:1745",
            SearchPeUrl = "http://localhost:1747",
            ModTaller = true,
            ModLotes = true,
        };
        _comprobanteDto.SetConfiguration(_configuration);
    }

    [TestMethod]
    public void Test_GetInvoiceSale()
    {
        var invoiceSale = _comprobanteDto.GetInvoiceSale();

        Assert.AreEqual("PEN", invoiceSale.TipMoneda);
        Assert.AreEqual(76.2712M, Math.Round(invoiceSale.SumTotTributos, 4));
        Assert.AreEqual(423.7288M, Math.Round(invoiceSale.SumTotValVenta, 4));
        Assert.AreEqual(500, invoiceSale.SumPrecioVenta);
        Assert.AreEqual(500, invoiceSale.SumImpVenta);
    }

    [TestMethod]
    public void Test_GetInvoiceSaleDetails()
    {
        var saleDetails = _comprobanteDto.GetInvoiceSaleDetails("625ecdb31f6bf6e6b66a2685");

        Assert.AreEqual(2, saleDetails.Count);
    }

    [TestMethod]
    public void Test_GetTributoSales()
    {
        _comprobanteDto.GetInvoiceSaleDetails("625ecdb31f6bf6e6b66a2685");
        var tributos = _comprobanteDto.GetTributoSales("625ecdb31f6bf6e6b66a2685");

        Assert.AreEqual(1, tributos.Count);
        Assert.AreEqual("1000", tributos[0].IdeTributo);
        Assert.AreEqual(423.7288M, Math.Round(tributos[0].MtoBaseImponible, 4));
        Assert.AreEqual(76.2712M, Math.Round(tributos[0].MtoTributo, 4));
    }

    [TestMethod]
    public void Test_GetDetallePagos()
    {
        var detallePagos = _comprobanteDto.GetDetallePagos("625ecdb31f6bf6e6b66a2685");

        Assert.AreEqual(0, detallePagos.Count);
    }

    [TestMethod]
    public void Test_GenerarSerieComprobante()
    {
        var invoiceSerie = new InvoiceSerie()
        {
            Id = "630461a24947a22864ef9652",
            Name = "TERMINAL-0",
            WarehouseId = "625e24bff2282d217b0644a1",
            WarehouseName = "TIENDA PRINCIPAL",
            NotaDeVenta = "N001",
            CounterNotaDeVenta = 460,
            Boleta = "B001",
            CounterBoleta = 46,
            Factura = "F001",
            CounterFactura = 127,
            CreditNoteBoleta = "BC01",
            CounterCreditNoteBoleta = 9,
            CreditNoteFactura = "FC01",
            CounterCreditNoteFactura = 9,
        };
        var invoiceSale = _comprobanteDto.GetInvoiceSale();
        _comprobanteDto.GenerarSerieComprobante(ref invoiceSerie, ref invoiceSale);

        Assert.AreEqual(47, invoiceSerie.CounterBoleta);
        Assert.AreEqual("B001", invoiceSale.Serie);
        Assert.AreEqual("00000047", invoiceSale.Number);
    }

}
