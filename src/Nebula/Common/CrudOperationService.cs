using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common.Models;
using Nebula.Database;

namespace Nebula.Common;

/// <summary>
/// Proporciona métodos genéricos para llevar a cabo operaciones CRUD (Create, Read, Update, Delete) en cualquier colección de la base de datos MongoDB.
/// </summary>
/// <typeparam name="T">Tipo de objeto que se va a manipular</typeparam>
public class CrudOperationService<T> where T : class, IGenericModel
{
    protected readonly MongoClient mongoClient;
    protected readonly IMongoCollection<T> _collection;

    /// <summary>
    /// rea una nueva instancia de la clase "CrudOperationService".
    /// </summary>
    /// <param name="options">Opciones de configuración de la base de datos</param>
    public CrudOperationService(IOptions<DatabaseSettings> options)
    {
        mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
    }

    /// <summary>
    /// Devuelve la lista de elementos de una colección que cumplan con el filtro proporcionado.
    /// </summary>
    /// <param name="field">Nombre del campo a filtrar</param>
    /// <param name="query">Texto para filtrar</param>
    /// <param name="limit">Cantidad máxima de elementos a devolver. Valor por defecto: 25.</param>
    /// <returns>Lista de elementos que cumplen con el filtro</returns>
    public virtual async Task<List<T>> GetAsync(string field, string? query, int limit = 25)
    {
        var filter = Builders<T>.Filter.Empty;
        if (!string.IsNullOrWhiteSpace(query))
            filter = Builders<T>.Filter.Regex(field, new BsonRegularExpression(query.ToUpper(), "i"));
        return await _collection.Find(filter).Limit(limit).ToListAsync();
    }

    /// <summary>
    /// Obtiene un item de la colección mediante su identificador único.
    /// </summary>
    /// <param name="id">Identificador único del item</param>
    /// <returns>El item encontrado o null si no se encuentra</returns>
    public virtual async Task<T> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Crea un nuevo registro de un documento en la colección.
    /// </summary>
    /// <param name="obj">Los datos del documento a crear</param>
    /// <returns>El documento creado con su correspondiente ID</returns>
    public virtual async Task<T> CreateAsync(T obj)
    {
        obj.Id = string.Empty;
        await _collection.InsertOneAsync(obj);
        return obj;
    }

    /// <summary>
    ///  Registra una lista de documentos en la base de datos.
    /// </summary>
    /// <param name="objList">Lista de documentos a registrar</param>
    /// <returns>Una tarea que representa la operación asincrónica de inserción de varios documentos.</returns>
    public virtual async Task InsertManyAsync(List<T> objList) =>
        await _collection.InsertManyAsync(objList);

    /// <summary>
    /// Actualiza un documento existente en la base de datos con los nuevos datos proporcionados.
    /// </summary>
    /// <param name="id">Identificador único del documento a actualizar</param>
    /// <param name="obj">Objeto con los nuevos datos para el documento</param>
    /// <returns>El documento actualizado</returns>
    public virtual async Task<T> UpdateAsync(string id, T obj)
    {
        await _collection.ReplaceOneAsync(x => x.Id == id, obj);
        return obj;
    }

    /// <summary>
    /// Elimina un documento de la colección por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del documento que se va a eliminar.</param>
    /// <returns>Nada.</returns>
    public virtual async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}
