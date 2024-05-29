using Microsoft.Extensions.Options;
using Nebula.Modules.InvoiceHub.Dto;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Nebula.Modules.InvoiceHub;

public interface IEmpresaHubService
{
    Task<string> RegistrarEmpresa(EmpresaHub empresa);
}

public class EmpresaHubService(HttpClient httpClient, IOptions<InvoiceHubSettings> settings)
    : IEmpresaHubService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly InvoiceHubSettings _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    public async Task<string> RegistrarEmpresa(EmpresaHub empresa)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.JwtToken);
            string url = $"{_settings.ApiBaseUrl}/api/configuration/registrarEmpresa";

            var jsonContent = new StringContent(JsonSerializer.Serialize(empresa), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return "Empresa registrada correctamente.";
            }
            else
            {
                return $"Error al registrar la empresa. Código de estado: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            return $"Error al registrar la empresa: {ex.Message}";
        }
    }
}
