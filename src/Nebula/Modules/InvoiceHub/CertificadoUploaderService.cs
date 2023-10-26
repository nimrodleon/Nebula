using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Nebula.Modules.InvoiceHub;

public interface ICertificadoUploaderService
{
    Task<string> SubirCertificado(byte[] certificadoPfx, string password, string ruc, string companyId);
}

public class CertificadoUploaderService : ICertificadoUploaderService
{
    private readonly HttpClient _httpClient;
    private readonly InvoiceHubSettings _settings;

    public CertificadoUploaderService(HttpClient httpClient, IOptions<InvoiceHubSettings> settings)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<string> SubirCertificado(byte[] certificadoPfx, string password, string ruc, string companyId)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.JwtToken);
            string url = $"{_settings.ApiBaseUrl}/api/configuration/subirCertificado";
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(ruc), "ruc");
            formData.Add(new StringContent(companyId), "companyId");
            formData.Add(new ByteArrayContent(certificadoPfx), "certificate", "cert.pem");
            formData.Add(new StringContent(password), "password");
            var response = await _httpClient.PostAsync(url, formData);

            if (response.IsSuccessStatusCode)
            {
                return "Certificado subido correctamente.";
            }
            else
            {
                return $"Error al subir el certificado. Código de estado: {response.StatusCode}";
            }
        }
        catch (Exception ex)
        {
            return $"Error al subir el certificado: {ex.Message}";
        }
    }
}
