using DeviceId;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Database.Dto;
using Nebula.Database.Models.Common;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Nebula.Database.Services.Common;

public class ConfigurationService
{
    private readonly IMongoCollection<Configuration> _collection;
    private readonly byte[] key = Encoding.ASCII.GetBytes("secret");
    private readonly byte[] IV = Encoding.ASCII.GetBytes("bsxnWolsAyO7kCfWuyrnqg==");

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

    public async Task UpdateAsync(Configuration configuration)
    {
        var _configuration = await GetAsync();
        _configuration.Ruc = configuration.Ruc;
        _configuration.RznSocial = configuration.RznSocial;
        _configuration.Address = configuration.Address;
        _configuration.PhoneNumber = configuration.PhoneNumber;
        _configuration.AnchoTicket = configuration.AnchoTicket;
        _configuration.CodLocalEmisor = configuration.CodLocalEmisor;
        _configuration.TipMoneda = configuration.TipMoneda;
        _configuration.PorcentajeIgv = configuration.PorcentajeIgv;
        _configuration.ValorImpuestoBolsa = configuration.ValorImpuestoBolsa;
        _configuration.CpeSunat = configuration.CpeSunat;
        _configuration.ModoEnvioSunat = configuration.ModoEnvioSunat;
        _configuration.ContactId = configuration.ContactId;
        _configuration.DiasPlazo = configuration.DiasPlazo;
        _configuration.SunatArchivos = configuration.SunatArchivos;
        await _collection.ReplaceOneAsync(x => x.Id == "DEFAULT", _configuration);
    }

    private string GetMachineUUID()
    {
        return new DeviceIdBuilder()
            .OnWindows(windows => windows
            .AddProcessorId()
            .AddMotherboardSerialNumber()
            .AddSystemDriveSerialNumber()
            .AddMacAddressFromWmi(excludeWireless: true, excludeNonPhysical: true))
            .ToString();
    }

    public async Task<string> UpdateAccess(string subscriptionId)
    {
        var machine = GetMachineUUID();
        HttpClient httpClient = new HttpClient();
        string URL = $"https://rd4lab.com/rbot/api/subscription/machine/{subscriptionId}";
        using StringContent jsonContent = new(JsonSerializer.Serialize(new { machine = machine }), Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await httpClient.PostAsync(URL, jsonContent);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        return jsonResponse;
    }

    public async Task<ResponseSubscriptionPaymentDto?> ValidarPago(string subscriptionId)
    {
        var machine = GetMachineUUID();
        HttpClient httpClient = new HttpClient();
        string URL = $"https://rd4lab.com/rbot/api/subscription/validation/{subscriptionId}";
        using StringContent jsonContent = new(JsonSerializer.Serialize(new { machine = machine }), Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await httpClient.PostAsync(URL, jsonContent);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResponseSubscriptionPaymentDto>(jsonResponse);
    }

    public async Task<ResponseSubscriptionPaymentDto?> SincronizarPago()
    {
        var configuration = await GetAsync();
        var pago = await ValidarPago(configuration.SubscriptionId);
        if (pago?.Ok == true)
        {
            string plainText = $"{pago.Data.Desde}:{pago.Data.Hasta}";
            var encrypt = EncryptStringToBytes(plainText, key, IV);
            configuration.AccessToken = pago.Data.Payment == true ? Encoding.Default.GetString(encrypt) : "-";
            await _collection.ReplaceOneAsync(x => x.Id == "DEFAULT", configuration);
        }
        return pago;
    }

    public async Task<Configuration> UpdateKey(string subscriptionId)
    {
        var configuration = await GetAsync();
        configuration.SubscriptionId = subscriptionId;
        var pago = await ValidarPago(configuration.SubscriptionId);
        if (pago?.Ok == true)
        {
            string plainText = $"{pago.Data.Desde}:{pago.Data.Hasta}";
            var encrypt = EncryptStringToBytes(plainText, key, IV);
            configuration.AccessToken = pago.Data.Payment == true ? Encoding.Default.GetString(encrypt) : "-";
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
