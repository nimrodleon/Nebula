using Nebula.Common.Helpers;
using Nebula.Modules.Configurations.Dto;
using Nebula.Modules.Configurations.Models;
using System.Text;
using System.Text.Json;

namespace Nebula.Modules.Configurations.Subscriptions;

public interface ISubscriptionService
{
    Task<LicenseDto> ValidarAcceso();
    Task<ResponseSubscriptionPaymentDto?> SincronizarPago();
    Task<Configuration> UpdateKey(string subscriptionId);
}

public class SubscriptionService : ISubscriptionService
{
    private readonly ILogger<SubscriptionService> _logger;
    private readonly IConfigurationService _configurationService;

    //##https://www.allkeysgenerator.com/Random/Security-Encryption-Key-Generator.aspx
    private readonly byte[] key = Encoding.ASCII.GetBytes("q3t6w9z$C&F)J@NcRfUjWnZr4u7x!A%D");
    private readonly byte[] IV = Encoding.ASCII.GetBytes("2r5u8x/A?D(G+KaP");

    public SubscriptionService(IConfigurationService configurationService,
        ILogger<SubscriptionService> logger)
    {
        _configurationService = configurationService;
        _logger = logger;
    }

    public async Task<LicenseDto> ValidarAcceso()
    {
        var machine = new MachineUUID().GetUUID();
        var licenseDto = new LicenseDto();
        var configuration = await _configurationService.GetAsync();
        if (configuration.AccessToken == "-") return new LicenseDto { Ok = false, OriginalText = "-" };
        byte[] cipherText = Convert.FromBase64String(configuration.AccessToken);
        licenseDto.OriginalText = TextEncryptor.DecryptText(cipherText, key, IV);
        string machineDb = licenseDto.OriginalText.Split("|")[1].Trim();
        string[] arrFecha = licenseDto.OriginalText.Split("|")[0].Split(":");
        string fechaDesde = arrFecha[0];
        string fechaHasta = arrFecha[1];
        int diffDiasTranscurridos = DateTime.Now.Subtract(Convert.ToDateTime(fechaDesde)).Days;
        int diffDiasQueFalta = Convert.ToDateTime(fechaHasta).Subtract(DateTime.Now).Days;
        int diffCantidadDeDias = Convert.ToDateTime(fechaHasta).Subtract(Convert.ToDateTime(fechaDesde)).Days;
        licenseDto.Ok = diffDiasTranscurridos <= diffCantidadDeDias && diffDiasQueFalta >= 0;
        licenseDto.Ok = licenseDto.Ok && machine == machineDb;
        //_logger.LogInformation(machineDb);
        //_logger.LogInformation(machine);
        return licenseDto;
    }

    private async Task<string> UpdateAccess(string subscriptionId)
    {
        var machine = new MachineUUID().GetUUID();
        HttpClient httpClient = new HttpClient();
        string URL = $"https://rd4lab.com/rbot/api/subscription/machine/{subscriptionId}";
        using StringContent jsonContent = new(JsonSerializer.Serialize(new { machine }), Encoding.UTF8,
            "application/json");
        using HttpResponseMessage response = await httpClient.PostAsync(URL, jsonContent);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        return jsonResponse;
    }

    private async Task<ResponseSubscriptionPaymentDto?> ValidarPago(string subscriptionId)
    {
        var machine = new MachineUUID().GetUUID();
        HttpClient httpClient = new HttpClient();
        string URL = $"https://rd4lab.com/rbot/api/subscription/validation/{subscriptionId}";
        using StringContent jsonContent = new(JsonSerializer.Serialize(new { machine }), Encoding.UTF8,
            "application/json");
        using HttpResponseMessage response = await httpClient.PostAsync(URL, jsonContent);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResponseSubscriptionPaymentDto>(jsonResponse,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }

    public async Task<ResponseSubscriptionPaymentDto?> SincronizarPago()
    {
        var configuration = await _configurationService.GetAsync();
        var pago = await ValidarPago(configuration.SubscriptionId);
        if (pago?.Ok == true)
        {
            var machine = new MachineUUID().GetUUID();
            string plainText = $"{pago.Data.Desde}:{pago.Data.Hasta}|{machine}";
            var encrypt = TextEncryptor.EncryptText(plainText, key, IV);
            configuration.AccessToken = pago.Data.Payment == true ? Convert.ToBase64String(encrypt) : "-";
            await _configurationService.ReplaceOneAsync(configuration);
        }

        return pago;
    }

    public async Task<Configuration> UpdateKey(string subscriptionId)
    {
        var configuration = await _configurationService.GetAsync();
        configuration.SubscriptionId = subscriptionId.Trim();
        await UpdateAccess(configuration.SubscriptionId);
        var pago = await ValidarPago(configuration.SubscriptionId);
        if (pago?.Ok != true)
            configuration.AccessToken = "-";
        else
        {
            var machine = new MachineUUID().GetUUID();
            string plainText = $"{pago.Data.Desde}:{pago.Data.Hasta}|{machine}";
            var encrypt = TextEncryptor.EncryptText(plainText, key, IV);
            configuration.AccessToken = pago.Data.Payment == true ? Convert.ToBase64String(encrypt) : "-";
        }

        await _configurationService.ReplaceOneAsync(configuration);
        return configuration;
    }

}
