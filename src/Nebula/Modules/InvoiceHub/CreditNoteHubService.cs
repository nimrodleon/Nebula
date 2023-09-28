using Microsoft.Extensions.Options;
using Nebula.Modules.InvoiceHub.Dto;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Nebula.Modules.InvoiceHub;

public interface ICreditNoteHubService
{
    Task<BillingResponse> SendCreditNoteAsync(CreditNoteRequestHub creditNoteRequest);
}

public class CreditNoteHubService : ICreditNoteHubService
{
    private readonly HttpClient _httpClient;
    private readonly InvoiceHubSettings _settings;
    private readonly ILogger<CreditNoteHubService> _logger;

    public CreditNoteHubService(HttpClient httpClient,
        IOptions<InvoiceHubSettings> settings, ILogger<CreditNoteHubService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger;
    }

    public async Task<BillingResponse> SendCreditNoteAsync(CreditNoteRequestHub creditNoteRequest)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.JwtToken);
            string url = $"{_settings.ApiBaseUrl}/api/creditNote/send";
            string jsonCreditNoteRequest = JsonSerializer.Serialize(creditNoteRequest);
            HttpContent content = new StringContent(jsonCreditNoteRequest, Encoding.UTF8, "application/json");
            _logger.LogInformation("Remote URL Invoice: " + url);
            _logger.LogInformation(jsonCreditNoteRequest.ToString());
            var response = await _httpClient.PostAsync($"{_settings.ApiBaseUrl}/api/creditNote/send", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<BillingResponse>();

                if (responseData != null && responseData.Success)
                {
                    return responseData;
                }
                else
                {
                    // Manejar errores aquí si la respuesta no es exitosa.
                    return new BillingResponse { Success = false, CdrDescription = "La respuesta no fue exitosa" };
                }
            }
            else
            {
                // Manejar errores aquí según sea necesario.
                return new BillingResponse { Success = false, CdrDescription = $"Error sending creditNote: {response.StatusCode}" };
            }
        }
        catch (Exception ex)
        {
            return new BillingResponse { Success = false, CdrDescription = $"Error sending creditNote: {ex.Message}" };
        }
    }
}
