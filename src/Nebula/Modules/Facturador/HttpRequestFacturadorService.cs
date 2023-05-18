using System.Net;
using System.Text;
using System.Text.Json;
using Nebula.Modules.Facturador.Bandeja;

namespace Nebula.Modules.Facturador;

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
    [Obsolete("Este método está obsoleto.")]
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

    /// <summary>
    /// Genera el XML del comprobante electrónico.
    /// </summary>
    /// <param name="tipDocu">FacturadorTipDocu</param>
    /// <returns>BandejaFacturador|null</returns>
    public async Task<BandejaFacturador?> GenerarComprobante(FacturadorTipDocu tipDocu)
    {
        var data = JsonSerializer.Serialize(tipDocu);
        _logger.LogInformation($"Parámetro del Método - {data}");
        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync($"{_facturadorUrl}/api/GenerarComprobante.htm", content);
        var result = await httpResponse.Content.ReadAsStringAsync();
        var deserializeData = JsonSerializer.Deserialize<BandejaFacturador>(result);
        _logger.LogInformation($"GenerarComprobante - {JsonSerializer.Serialize(deserializeData)}");
        return deserializeData;
    }

    /// <summary>
    /// Envía el comprobante a la SUNAT.
    /// </summary>
    /// <param name="tipDocu">FacturadorTipDocu</param>
    /// <returns>BandejaFacturador|null</returns>
    public async Task<BandejaFacturador?> EnviarXml(FacturadorTipDocu tipDocu)
    {
        var data = JsonSerializer.Serialize(tipDocu);
        _logger.LogInformation($"Parámetro del Método - {data}");
        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync($"{_facturadorUrl}/api/enviarXML.htm", content);
        var result = await httpResponse.Content.ReadAsStringAsync();
        var deserializeData = JsonSerializer.Deserialize<BandejaFacturador>(result);
        _logger.LogInformation($"EnviarXml - {JsonSerializer.Serialize(deserializeData)}");
        return deserializeData;
    }

    /// <summary>
    /// Genera el comprobante electrónico en Pdf.
    /// </summary>
    /// <param name="tipDocu">FacturadorTipDocu</param>
    /// <returns>True|False</returns>
    [Obsolete("Este método está obsoleto.")]
    public async Task<bool> MostrarXml(FacturadorTipDocu tipDocu)
    {
        // formar la cadena: 20520485750-03-B002-00000122
        string nomArch = $"{tipDocu.num_ruc}-{tipDocu.tip_docu}-{tipDocu.num_docu}";
        var data = JsonSerializer.Serialize(new { nomArch });
        _logger.LogInformation($"Parámetro del Método - {data}");
        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        var httpResponse = await _httpClient.PostAsync($"{_facturadorUrl}/api/MostrarXml.htm", content);
        return httpResponse.StatusCode == HttpStatusCode.Created;
    }
}
