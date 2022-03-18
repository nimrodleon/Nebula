using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Nebula.Data;

public interface IRavenDbContext
{
    public IDocumentStore Store { get; }
}

public class RavenDbContext : IRavenDbContext
{
    private readonly DocumentStore _localStore;
    public IDocumentStore Store => _localStore;
    private readonly PersistenceSettings _persistenceSettings;

    public RavenDbContext(IOptionsMonitor<PersistenceSettings> settings)
    {
        _persistenceSettings = settings.CurrentValue;
        _localStore = new DocumentStore()
        {
            Database = _persistenceSettings.DatabaseName,
            Urls = _persistenceSettings.Urls
        };
        _localStore.Initialize();
        EnsureDatabaseIsCreated();
    }

    private void EnsureDatabaseIsCreated()
    {
        try
        {
            _localStore.Maintenance.ForDatabase(_persistenceSettings.DatabaseName)
                .Send(new GetStatisticsOperation());
        }
        catch (DatabaseDoesNotExistException)
        {
            _localStore.Maintenance.Server.Send(
                new CreateDatabaseOperation(new DatabaseRecord(_persistenceSettings.DatabaseName)));
        }
    }
}
