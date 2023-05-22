using DeviceId;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Database.Dto;
using Nebula.Modules.Configurations.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Nebula.Modules.Configurations;

public class ConfigurationService
{
    private readonly IMongoCollection<Configuration> _collection;

    //##https://www.allkeysgenerator.com/Random/Security-Encryption-Key-Generator.aspx
    private readonly byte[] key = Encoding.ASCII.GetBytes("q3t6w9z$C&F)J@NcRfUjWnZr4u7x!A%D");
    private readonly byte[] IV = Encoding.ASCII.GetBytes("2r5u8x/A?D(G+KaP");

    public ConfigurationService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Configuration>("Configuration");
    }

    public async Task<Configuration> GetAsync() =>
        await _collection.Find(x => x.Id == "DEFAULT").FirstOrDefaultAsync();

    public async Task CreateAsync() =>
        await _collection.InsertOneAsync(new Configuration());

    public async Task<Configuration> UpdateAsync(Configuration configuration)
    {
        var _configuration = await GetAsync();
        _configuration.Ruc = configuration.Ruc.Trim();
        _configuration.RznSocial = configuration.RznSocial.Trim();
        _configuration.Address = configuration.Address.Trim();
        _configuration.PhoneNumber = configuration.PhoneNumber;
        _configuration.AnchoTicket = configuration.AnchoTicket;
        _configuration.CodLocalEmisor = configuration.CodLocalEmisor.Trim();
        _configuration.TipMoneda = configuration.TipMoneda.Trim();
        _configuration.PorcentajeIgv = configuration.PorcentajeIgv;
        _configuration.ValorImpuestoBolsa = configuration.ValorImpuestoBolsa;
        _configuration.CpeSunat = configuration.CpeSunat;
        _configuration.ModoEnvioSunat = configuration.ModoEnvioSunat;
        _configuration.ContactId = configuration.ContactId;
        _configuration.DiasPlazo = configuration.DiasPlazo;
        _configuration.ModTaller = configuration.ModTaller;
        _configuration.ModLotes = configuration.ModLotes;
        await _collection.ReplaceOneAsync(x => x.Id == "DEFAULT", _configuration);
        return _configuration;
    }

    public async Task<LicenseDto> ValidarAcceso()
    {
        var licenseDto = new LicenseDto();
        var configuration = await GetAsync();
        if (configuration.AccessToken == "-") return new LicenseDto { Ok = false, OriginalText = "-" };
        byte[] cipherText = Convert.FromBase64String(configuration.AccessToken);
        licenseDto.OriginalText = DecryptStringFromBytes(cipherText, key, IV);
        string[] arrFecha = licenseDto.OriginalText.Split(":");
        string fechaDesde = arrFecha[0];
        string fechaHasta = arrFecha[1];
        int diffDiasTranscurridos = DateTime.Now.Subtract(Convert.ToDateTime(fechaDesde)).Days;
        int diffDiasQueFalta = Convert.ToDateTime(fechaHasta).Subtract(DateTime.Now).Days;
        int diffCantidadDeDias = Convert.ToDateTime(fechaHasta).Subtract(Convert.ToDateTime(fechaDesde)).Days;
        licenseDto.Ok = diffDiasTranscurridos <= diffCantidadDeDias && diffDiasQueFalta >= 0;
        return licenseDto;
    }

    private string GetMachineUUID()
    {
        string deviceId = new DeviceIdBuilder()
            .AddMachineName().AddUserName()
            .AddMacAddress(excludeWireless: true)
            .AddOsVersion().ToString();
        return deviceId;
    }

    private async Task<string> UpdateAccess(string subscriptionId)
    {
        var machine = GetMachineUUID();
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
        var machine = GetMachineUUID();
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
        var configuration = await GetAsync();
        var pago = await ValidarPago(configuration.SubscriptionId);
        if (pago?.Ok == true)
        {
            string plainText = $"{pago.Data.Desde}:{pago.Data.Hasta}";
            var encrypt = EncryptStringToBytes(plainText, key, IV);
            configuration.AccessToken = pago.Data.Payment == true ? Convert.ToBase64String(encrypt) : "-";
            await _collection.ReplaceOneAsync(x => x.Id == "DEFAULT", configuration);
        }

        return pago;
    }

    public async Task<Configuration> UpdateKey(string subscriptionId)
    {
        var configuration = await GetAsync();
        configuration.SubscriptionId = subscriptionId.Trim();
        await UpdateAccess(configuration.SubscriptionId);
        var pago = await ValidarPago(configuration.SubscriptionId);
        if (pago?.Ok != true)
            configuration.AccessToken = "-";
        else
        {
            string plainText = $"{pago.Data.Desde}:{pago.Data.Hasta}";
            var encrypt = EncryptStringToBytes(plainText, key, IV);
            configuration.AccessToken = pago.Data.Payment == true ? Convert.ToBase64String(encrypt) : "-";
        }

        await _collection.ReplaceOneAsync(x => x.Id == "DEFAULT", configuration);
        return configuration;
    }

    private byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    private string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string? plaintext = null;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}
