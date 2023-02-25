using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nebula.Data;

namespace Nebula.Services;

/// <summary>
/// Clase Genérica para realizar Operaciones Crud.
/// </summary>
/// <typeparam name="T">Entidad</typeparam>
public class CrudOperationService<T> where T : class, IGenericDocument
{
    private readonly IMongoCollection<T> _collection;

    /// <summary>
    /// Constructor de la Clase.
    /// </summary>
    /// <param name="options">Carga la cadena de conexión a la DB</param>
    public CrudOperationService(IOptions<DatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
    }

    /// <summary>
    /// Obtener un documento mongoDb mediante su ID
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns>Documento de la Colección</returns>
    public async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Crea un Documento con los datos de la Entidad.
    /// </summary>
    /// <param name="entity">Instancia de Clase [T]</param>
    /// <returns>Datos Insertados en la BD</returns>
    public async Task<T> CreateAsync(T entity)
    {
        entity.Id = string.Empty;
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    /// <summary>
    /// Actualiza un Documento en la BD.
    /// </summary>
    /// <param name="id">Id del documento</param>
    /// <param name="entity">Instancia de Clase [T]</param>
    /// <returns>True|False</returns>
    public async Task<bool> UpdateAsync(string id, T entity)
    {
        var result = await _collection.ReplaceOneAsync(x =>
            x.Id == id, entity, new ReplaceOptions() { IsUpsert = true });
        return result.ModifiedCount > 0;
    }

    /// <summary>
    /// Elimina un Documento en la BD.
    /// </summary>
    /// <param name="id">Id del documento</param>
    /// <returns>True|False</returns>
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
