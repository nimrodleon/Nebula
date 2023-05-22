using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;
using Nebula.Modules.Inventory.Helpers;

namespace Nebula.Modules.Inventory.Models;

[BsonIgnoreExtraElements]
public class Material : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Usuario Autentificado.
    /// </summary>
    public string User { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de Contacto.
    /// </summary>
    public string ContactId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de Contacto.
    /// </summary>
    public string ContactName { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Trabajador.
    /// </summary>
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Trabajador.
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// Estado del Inventario.
    /// </summary>
    public string Status { get; set; } = InventoryStatus.BORRADOR;

    /// <summary>
    /// Comentario del Inventario.
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de Registro.
    /// </summary>
    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

    /// <summary>
    /// Año de Registro.
    /// </summary>
    public string Year { get; set; } = DateTime.Now.ToString("yyyy");

    /// <summary>
    /// Mes de Registro.
    /// </summary>
    public string Month { get; set; } = DateTime.Now.ToString("MM");
}
