using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Modules.Hoteles.Models;

[BsonIgnoreExtraElements]
public class HabitacionHotel : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "CompanyId es requerido.")]
    public string CompanyId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string PisoHotelId { get; set; } = string.Empty;
    public string CategoriaHabitacionId { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int TarifaHoras { get; set; }
    public string Remark { get; set; } = string.Empty;
}
