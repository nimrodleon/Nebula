using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Configurations.Models;

namespace Nebula.Modules.Configurations;

public interface IConfigurationService
{
    Task<Configuration> GetAsync();
    Task CreateAsync();
    Task ReplaceOneAsync(Configuration configuration);
    Task<Configuration> UpdateAsync(Configuration configuration);
}

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoCollection<Configuration> _collection;

    public ConfigurationService(IOptions<DatabaseSettings> options, IConfiguration configuration)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<Configuration>("Configuration");
        _configuration = configuration;
    }

    public async Task<Configuration> GetAsync()
    {
        var configuration = await _collection.Find(x => x.Id == "DEFAULT").FirstOrDefaultAsync();
        configuration.FacturadorUrl = _configuration.GetValue<string>("facturadorUrl");
        configuration.SearchPeUrl = _configuration.GetValue<string>("searchPeUrl");
        return configuration;
    }

    public async Task CreateAsync() =>
        await _collection.InsertOneAsync(new Configuration());

    public async Task ReplaceOneAsync(Configuration configuration) =>
        await _collection.ReplaceOneAsync(x => x.Id == "DEFAULT", configuration);

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
        _configuration.ModInventories = configuration.ModInventories;
        _configuration.ModTaller = configuration.ModTaller;
        _configuration.ModLotes = configuration.ModLotes;
        await _collection.ReplaceOneAsync(x => x.Id == "DEFAULT", _configuration);
        return _configuration;
    }
}
