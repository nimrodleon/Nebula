using Microsoft.Extensions.Options;
using Nebula.Modules.InvoiceHub.Dto;
using Nebula.Modules.InvoiceHub;

namespace Testings.InvoiceHub;

[TestClass]
public class InvoiceHubServiceTests
{
    [TestMethod]
    public async Task SendInvoiceAsync_SuccessfulResponse_ReturnsBillingResponse()
    {
        // Arrange
        var settings = new InvoiceHubSettings
        {
            ApiBaseUrl = "http://127.0.0.1:8000",
            JwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoibmVidWxhIiwidXNlcm5hbWUiOm51bGwsImV4cCI6MTcyNDAyMTE5MH0.b5rpe2T3OvbCr8I4d5LIVycCItaTixEOtBc8uMGPhw0",
        };

        var options = Options.Create(settings);
        var invoiceHubService = new InvoiceHubService(new HttpClient(), options);

        // Act
        var invoiceRequest = new InvoiceRequestHub
        {
            ruc = "20520485750",
            TipoOperacion = "0101",
            TipoDoc = "01",
            Serie = "F001",
            Correlativo = "1",
            FormaPago = new FormaPagoHub
            {
                Moneda = "PEN",
                Tipo = "Contado",
                Monto = 100,
            },
            TipoMoneda = "PEN",
            Client = new ClientHub
            {
                TipoDoc = "6",
                NumDoc = "10234545455",
                RznSocial = "Cliente Ejemplo SRL",
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
                    MtoPrecioUnitario = 59,
                },
            },
        };

        var billingResponse = await invoiceHubService.SendInvoiceAsync(invoiceRequest);

        // Assert
        Assert.IsNotNull(billingResponse);
        Assert.IsTrue(billingResponse.Success);
    }

    [TestMethod]
    public async Task SendInvoiceAsync_UnsuccessfulResponse_ReturnsBillingResponseWithError()
    {
        // Arrange
        var settings = new InvoiceHubSettings
        {
            ApiBaseUrl = "http://127.0.0.1:8000",
            JwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoibmVidWxhIiwidXNlcm5hbWUiOm51bGwsImV4cCI6MTcyNDAyMTE5MH0.b5rpe2T3OvbCr8I4d5LIVycCItaTixEOtBc8uMGPhw0",
        };

        var options = Options.Create(settings);

        var invoiceHubService = new InvoiceHubService(new HttpClient(), options);

        // Act
        var invoiceRequest = new InvoiceRequestHub
        {
            ruc = "20520485750",
            TipoOperacion = "0101",
            TipoDoc = "01",
            Serie = "F001",
            Correlativo = "{{correlativo}}",
            FormaPago = new FormaPagoHub
            {
                Moneda = "PEN",
                Tipo = "Contado",
                Monto = 100,
            },
            TipoMoneda = "PEN",
            Client = new ClientHub
            {
                TipoDoc = "6",
                NumDoc = "10234545455",
                RznSocial = "Cliente Ejemplo SRL",
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
                    MtoPrecioUnitario = 59,
                },
            },
        };

        var billingResponse = await invoiceHubService.SendInvoiceAsync(invoiceRequest);

        // Assert
        Assert.IsNotNull(billingResponse);
        Assert.IsFalse(billingResponse.Success);
    }
}
