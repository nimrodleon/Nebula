using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nebula.Plugins.Facturador;
using Nebula.Plugins.Facturador.Dto;
using Moq;

namespace Testings.Plugins;

/// <summary>
/// Para esta prueba debe existir al menos
/// un archivo en la carpeta DATA del facturador.
/// </summary>
[TestClass]
public class HttpRequestFacturadorServiceTests
{
    private readonly IConfigurationRoot _configuration;
    private readonly Mock<ILogger<HttpRequestFacturadorService>> _loggerMock;

    public HttpRequestFacturadorServiceTests()
    {
        var builder = new ConfigurationBuilder();
        var data = new Dictionary<string, string>();
        data.Add("facturadorUrl", "http://localhost:1745");
        builder.AddInMemoryCollection(data);
        _configuration = builder.Build();
        _loggerMock = new Mock<ILogger<HttpRequestFacturadorService>>();
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
        Assert.AreEqual("EXITO", result.validacion);
    }
}
