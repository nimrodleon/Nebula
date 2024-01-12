using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Modules.Hoteles.Models;

[BsonIgnoreExtraElements]
public class CategoriaHabitacion : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "CompanyId es requerido.")]
    public string CompanyId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
