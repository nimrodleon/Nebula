using Microsoft.Extensions.Options;
using Nebula.Modules.InvoiceHub.Dto;
using System.Net.Http.Headers;

namespace Nebula.Modules.InvoiceHub;

public interface IInvoiceHubService
{
    Task<BillingResponse> SendInvoiceAsync(InvoiceRequestHub invoiceRequest);
}

public class InvoiceHubService : IInvoiceHubService
{
    private readonly HttpClient _httpClient;
    private readonly InvoiceHubSettings _settings;

    public InvoiceHubService(HttpClient httpClient, IOptions<InvoiceHubSettings> settings)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<BillingResponse> SendInvoiceAsync(InvoiceRequestHub invoiceRequest)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.JwtToken);
            var response = await _httpClient.PostAsJsonAsync($"{_settings.ApiBaseUrl}/api/invoice/send", invoiceRequest);

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
