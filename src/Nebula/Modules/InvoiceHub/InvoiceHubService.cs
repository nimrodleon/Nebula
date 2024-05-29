using Microsoft.Extensions.Options;
using Nebula.Modules.InvoiceHub.Dto;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Nebula.Modules.InvoiceHub;

public interface IInvoiceHubService
{
    Task<BillingResponse> SendInvoiceAsync(string companyId, InvoiceRequestHub invoiceRequest);
}

public class InvoiceHubService(
    HttpClient httpClient,
    IOptions<InvoiceHubSettings> settings,
    ILogger<InvoiceHubService> logger)
    : IInvoiceHubService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly InvoiceHubSettings _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

    public async Task<BillingResponse> SendInvoiceAsync(string companyId, InvoiceRequestHub invoiceRequest)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.JwtToken);
            string url = $"{_settings.ApiBaseUrl}/api/invoice/send/{companyId.Trim()}";
            string jsonInvoiceRequest = JsonSerializer.Serialize(invoiceRequest);
            HttpContent content = new StringContent(jsonInvoiceRequest, Encoding.UTF8, "application/json");
            logger.LogInformation("Remote URL Invoice: " + url);
            logger.LogInformation(jsonInvoiceRequest.ToString());
            var response = await _httpClient.PostAsync(url, content);

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
                return new BillingResponse { Success = false, CdrDescription = $"Error sending invoice: {response.StatusCode}" };
            }
        }
        catch (Exception ex)
        {
            return new BillingResponse { Success = false, CdrDescription = $"Error sending invoice: {ex.Message}" };
        }
    }
}
