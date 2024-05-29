using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace Nebula.Modules.InvoiceHub;

public interface ICertificadoUploaderService
{
    Task<string> SubirCertificado(byte[] certificadoPfx, string password, string companyId, string extension);
}

public class CertificadoUploaderService(
    HttpClient httpClient,
    IOptions<InvoiceHubSettings> settings,
    ILogger<CertificadoUploaderService> logger)
    : ICertificadoUploaderService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly InvoiceHubSettings _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    public async Task<string> SubirCertificado(byte[] certificado, string password, string companyId, string extension)
    {
        try
        {
            if (extension == "p12") certificado = ConvertirP12APfx(certificado, password);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.JwtToken);
            string url = $"{_settings.ApiBaseUrl}/api/configuration/subirCertificado";
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(companyId), "companyId");
            formData.Add(new ByteArrayContent(certificado), "certificate", "cert.pfx");
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

    public byte[] ConvertirP12APfx(byte[] certificadoP12Bytes, string password)
    {
        try
        {
            // Crea un objeto X509Certificate2 desde el archivo P12 y la contraseña
            X509Certificate2 certificadoP12 = new X509Certificate2(certificadoP12Bytes, password,
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

            // Exporta el certificado a formato PFX (P12) en memoria
            byte[] certificadoPfxBytes = certificadoP12.Export(X509ContentType.Pfx, password);

            return certificadoPfxBytes;
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex.Message);
            return null;
        }
    }
}
