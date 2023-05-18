using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nebula.Modules.Facturador;
using Nebula.Modules.Facturador.Bandeja;
using Moq;

namespace Testings.Modules;

/// <summary>
/// Para esta prueba debe existir al menos un archivo en la carpeta DATA del facturador.
/// </summary>
[TestClass]
public class HttpRequestFacturadorServiceTests
{
    private readonly IConfigurationRoot _configuration;
    private readonly Mock<ILogger<HttpRequestFacturadorService>> _loggerMock;
    private readonly FacturadorTipDocu _facturadorTipDocu;

    public HttpRequestFacturadorServiceTests()
    {
        var builder = new ConfigurationBuilder();
        var data = new Dictionary<string, string>();
        data.Add("facturadorUrl", "http://localhost:1745");
        builder.AddInMemoryCollection(data);
        _configuration = builder.Build();
        _loggerMock = new Mock<ILogger<HttpRequestFacturadorService>>();
        _facturadorTipDocu = new FacturadorTipDocu()
        {
            num_ruc = "20520485750",
            tip_docu = "03",
            num_docu = "B001-00000008"
        };
    }

    [TestMethod]
    public async Task ActualizarPantalla_ShouldReturn_BandejaFacturador()
    {
        var httpRequestFacturadorService = new HttpRequestFacturadorService(_configuration, _loggerMock.Object);
        var result = await httpRequestFacturadorService.ActualizarPantalla();
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BandejaFacturador));
        Assert.AreEqual("EXITO", result.validacion);
        Assert.IsTrue(result.listaBandejaFacturador.Count > 0);
    }

    [TestMethod]
    public async Task EliminarBandeja_ShouldReturn_BandejaFacturador()
    {
        var httpRequestFacturadorService = new HttpRequestFacturadorService(_configuration, _loggerMock.Object);
        var result = await httpRequestFacturadorService.EliminarBandeja();
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BandejaFacturador));
        Assert.AreEqual("EXITO", result.validacion);
    }

    [TestMethod]
    public async Task GenerarComprobante_ShouldReturn_BandejaFacturador()
    {
        var httpRequestFacturadorService = new HttpRequestFacturadorService(_configuration, _loggerMock.Object);
        var result = await httpRequestFacturadorService.GenerarComprobante(_facturadorTipDocu);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BandejaFacturador));
        Assert.AreEqual("EXITO", result.validacion);
        Assert.IsTrue(result.listaBandejaFacturador.Count > 0);
    }

    [TestMethod]
    public async Task EnviarXml_ShouldReturn_BandejaFacturador()
    {
        var httpRequestFacturadorService = new HttpRequestFacturadorService(_configuration, _loggerMock.Object);
        var result = await httpRequestFacturadorService.EnviarXml(_facturadorTipDocu);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BandejaFacturador));
        Assert.AreEqual("EXITO", result.validacion);
        Assert.IsTrue(result.listaBandejaFacturador.Count > 0);
    }

    [TestMethod]
    public async Task CrearPdfComprobanteElectrónico()
    {
        var httpRequestFacturadorService = new HttpRequestFacturadorService(_configuration, _loggerMock.Object);
        var result = await httpRequestFacturadorService.MostrarXml(_facturadorTipDocu);
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.AreEqual(true, result);
    }
}
