using Nebula.Modules.Configurations.Models;
using Nebula.Modules.Purchases.Dto;
using Nebula.Modules.Purchases.Models;
using Nebula.Modules.Sales.Helpers;

namespace Testings.Modules.Purchases.Dto;

[TestClass]
public class PurchaseDataDtoTests
{
    private PurchaseDataDto _purchaseDataDto = new PurchaseDataDto();

    [TestInitialize]
    public void Initialize()
    {
        _purchaseDataDto.DetailDtos = new List<PurchaseDetailDto>() {
            new PurchaseDetailDto() {
                TipoItem = "BIEN",
                CtdUnidadItem = 4,
                DesItem = "Producto 1",
                TriIcbper = false,
                IgvSunat = TipoIGV.Gravado,
                ProductId = "P01",
                MtoPrecioCompraUnitario = 20
            },
            new PurchaseDetailDto() {
                TipoItem = "BIEN",
                CtdUnidadItem = 1,
                DesItem = "Producto 2",
                TriIcbper = false,
                IgvSunat = TipoIGV.Gravado,
                ProductId = "P01",
                MtoPrecioCompraUnitario = 68
            },
        };
        _purchaseDataDto.SetConfiguration(new Configuration()
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
            PadronReducidoRuc = true,
            ModTaller = true,
            ModLotes = true,
        });
    }

    [TestMethod]
    public void GetPurchaseInvoiceDetails_Test()
    {
        List<PurchaseInvoiceDetail> purchaseInvoiceDetails = _purchaseDataDto.GetPurchaseInvoiceDetails("1");

        Assert.IsNotNull(purchaseInvoiceDetails);
        Assert.AreEqual(2, purchaseInvoiceDetails.Count);
        var totalCantidades = purchaseInvoiceDetails.Sum(x => x.CtdUnidadItem);
        Assert.AreEqual(5, totalCantidades);
        // test valor unitario.
        Assert.AreEqual(16.9492M, Math.Round(purchaseInvoiceDetails[0].MtoValorUnitario, 4));
        Assert.AreEqual(57.6271M, Math.Round(purchaseInvoiceDetails[1].MtoValorUnitario, 4));
        // test sumatoria tributos por item.
        Assert.AreEqual(12.2034M, Math.Round(purchaseInvoiceDetails[0].SumTotTributosItem, 4));
        Assert.AreEqual(10.3729M, Math.Round(purchaseInvoiceDetails[1].SumTotTributosItem, 4));
    }
}
