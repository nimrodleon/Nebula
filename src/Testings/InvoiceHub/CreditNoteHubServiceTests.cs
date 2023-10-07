using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.InvoiceHub.Dto;

namespace Testings.InvoiceHub;

[TestClass]
public class CreditNoteHubServiceTests
{
    private ILogger<CreditNoteHubService> _logger;

    [TestInitialize]
    public void Initialize()
    {
        _logger = Mock.Of<ILogger<CreditNoteHubService>>();
    }

    [TestMethod]
    public async Task SendCreditNoteAsync_SuccessfulResponse_ReturnsBillingResponse()
    {
        // Arrange
        var settings = new InvoiceHubSettings
        {
            ApiBaseUrl = "http://127.0.0.1:8000",
            JwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoibmVidWxhIiwidXNlcm5hbWUiOm51bGwsImV4cCI6MTcyNDAyMTE5MH0.b5rpe2T3OvbCr8I4d5LIVycCItaTixEOtBc8uMGPhw0",
        };

        var options = Options.Create(settings);
        var creditNoteHubService = new CreditNoteHubService(new HttpClient(), options, _logger);

        // Act
        var creditNoteRequest = new CreditNoteRequestHub
        {
            Ruc = "20520485750",
            TipoDoc = "07",
            Serie = "FC01",
            Correlativo = "10",
            TipDocAfectado = "01",
            NumDocAfectado = "F001-111",
            CodMotivo = "07",
            DesMotivo = "DEVOLUCION POR ITEM",
            TipoMoneda = "PEN",
            Client = new ClientHub
            {
                TipoDoc = "6",
                NumDoc = "20000000001",
                RznSocial = "EMPRESA X"
            },
            Details = new List<DetailHub>
                {
                    new DetailHub
                    {
                        CodProducto = "P001",
                        Unidad = "NIU",
                        Cantidad = 2,
                        MtoValorUnitario = 50,
                        Descripcion = "Producto 1",
                        MtoBaseIgv = 100,
                        PorcentajeIgv = 18,
                        Igv = 18,
                        TipAfeIgv = "10",
                        TotalImpuestos = 18,
                        MtoValorVenta = 100,
                        MtoPrecioUnitario = 59
                    },
                    new DetailHub
                    {
                        CodProducto = "P002",
                        Unidad = "NIU",
                        Cantidad = 2,
                        MtoValorUnitario = 50,
                        Descripcion = "Producto 2",
                        MtoBaseIgv = 100,
                        PorcentajeIgv = 18,
                        Igv = 18,
                        TipAfeIgv = "10",
                        TotalImpuestos = 18,
                        MtoValorVenta = 100,
                        MtoPrecioUnitario = 59
                    }
                }
        };

        var billingResponse = await creditNoteHubService.SendCreditNoteAsync("651b559c6256e58372cde4c2", creditNoteRequest);

        // Assert
        Assert.IsNotNull(billingResponse);
        Assert.IsTrue(billingResponse.Success);
    }

    [TestMethod]
    public async Task SendCreditNoteAsync_UnsuccessfulResponse_ReturnsBillingResponseWithError()
    {
        // Arrange
        var settings = new InvoiceHubSettings
        {
            ApiBaseUrl = "http://127.0.0.1:8000",
            JwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoibmVidWxhIiwidXNlcm5hbWUiOm51bGwsImV4cCI6MTcyNDAyMTE5MH0.b5rpe2T3OvbCr8I4d5LIVycCItaTixEOtBc8uMGPhw0",
        };

        var options = Options.Create(settings);
        var creditNoteHubService = new CreditNoteHubService(new HttpClient(), options, _logger);

        // Act
        var creditNoteRequest = new CreditNoteRequestHub
        {
            Ruc = "20520485750",
            TipoDoc = "07",
            Serie = "FC01",
            Correlativo = "-",
            TipDocAfectado = "01",
            NumDocAfectado = "F001-111",
            CodMotivo = "07",
            DesMotivo = "DEVOLUCION POR ITEM",
            TipoMoneda = "PEN",
            Client = new ClientHub
            {
                TipoDoc = "6",
                NumDoc = "20000000001",
                RznSocial = "EMPRESA X"
            },
            Details = new List<DetailHub>
                {
                    new DetailHub
                    {
                        CodProducto = "P001",
                        Unidad = "NIU",
                        Cantidad = 2,
                        MtoValorUnitario = 50,
                        Descripcion = "Producto 1",
                        MtoBaseIgv = 100,
                        PorcentajeIgv = 18,
                        Igv = 18,
                        TipAfeIgv = "10",
                        TotalImpuestos = 18,
                        MtoValorVenta = 100,
                        MtoPrecioUnitario = 59
                    }
                }
        };

        var billingResponse = await creditNoteHubService.SendCreditNoteAsync("651b559c6256e58372cde4c2", creditNoteRequest);

        // Assert
        Assert.IsNotNull(billingResponse);
        Assert.IsFalse(billingResponse.Success);
    }
}
