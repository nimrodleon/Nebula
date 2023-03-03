using System.Text;
using System.Text.Json;
using Nebula.Plugins.Facturador.Dto;

namespace Nebula.Plugins.Facturador;

/// <summary>
/// ApiREST para manejar el Facturador SUNAT.
/// </summary>
public class HttpRequestFacturadorService
{
    private readonly string? _facturadorUrl;
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpRequestFacturadorService> _logger;

    /// <summary>
    /// Constructor de Clase.
    /// </summary>
    /// <param name="configuration">IConfiguration</param>
    public HttpRequestFacturadorService(IConfiguration configuration, ILogger<HttpRequestFacturadorService> logger)
    {
        _httpClient = new HttpClient();
        _facturadorUrl = configuration.GetValue<string>("facturadorUrl");
        _logger = logger;
    }

    /// <summary>
    /// Carga la Lista de comprobantes del facturador.
    /// </summary>
    /// <returns>BandejaFacturador|null</returns>
    public async Task<BandejaFacturador?> ActualizarPantalla()
    {
        var data = JsonSerializer.Serialize(new { });
        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync($"{_facturadorUrl}/api/ActualizarPantalla.htm", content);
        var result = await httpResponse.Content.ReadAsStringAsync();
        var deserializeData = JsonSerializer.Deserialize<BandejaFacturador>(result);
        _logger.LogInformation($"ActualizarPantalla - {JsonSerializer.Serialize(deserializeData)}");
        return deserializeData;
    }

    /// <summary>
    /// Elimina la Lista de comprobantes de la bandeja del facturador.
    /// </summary>
    /// <returns>BandejaFacturador|null</returns>
    public async Task<BandejaFacturador?> EliminarBandeja()
    {
        var data = JsonSerializer.Serialize(new { });
        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync($"{_facturadorUrl}/api/EliminarBandeja.htm", content);
        var result = await httpResponse.Content.ReadAsStringAsync();
        var deserializeData = JsonSerializer.Deserialize<BandejaFacturador>(result);
        _logger.LogInformation($"EliminarBandeja - {JsonSerializer.Serialize(deserializeData)}");
        return deserializeData;
    }
}
